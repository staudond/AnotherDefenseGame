using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public abstract class BasicEnemy: BasicCreature {
    
    protected int stamina = 15;
    protected List<Vector2Int> path;

    public List<Vector2Int> Path {
        get => path;
        set => path = value;
    }

    public int Stamina => stamina;

    public BasicEnemy(Vector2Int pos) : base(pos)
    {
    }
    
    protected override bool IndividualAttack() {
        BasicUnit target = null;
        int targetDistance = int.MaxValue;
        if (map[path[0].x, path[0].y].hasUnit) {
            
            return DoIndividualAttack(map[path[0].x, path[0].y].unit);
        }
        
        List<Vector2Int> attackPositions = GameManager.RangeVectorsToPositions(position,GameManager.AttackRangeToRangeVectors(AttackRange));
        
        
        foreach (Vector2Int pos in attackPositions) {
            if (manager.CheckBounds(pos) && !map[pos.x, pos.y].isEmpty && !map[pos.x, pos.y].hasEnemy) {
                if (path.Contains(pos)) {
                    int distance = CalculateDistance(pos);

                    if (distance < targetDistance) {
                        target = map[pos.x, pos.y].unit;
                        targetDistance = distance;
                    }
                    else if (distance == targetDistance && target.Health > map[pos.x, pos.y].unit.Health) {
                        target = map[pos.x, pos.y].unit;
                    }
                }
            }
        }

        if (target == null) {
            foreach (Vector2Int pos in attackPositions) {
                
                if (manager.CheckBounds(pos) && !map[pos.x, pos.y].isEmpty && !map[pos.x, pos.y].hasEnemy) {
                    int distance = CalculateDistance(pos);
                    if (distance < targetDistance) {
                        target = map[pos.x, pos.y].unit;
                        targetDistance = distance;
                    }
                    else if (distance == targetDistance && target.Health > map[pos.x, pos.y].unit.Health) {
                        target = map[pos.x, pos.y].unit;
                    }
                }
            }
        }
        
        
        
        return DoIndividualAttack(target);
       
    }

    public void Disappear() {
        //used when enemy reaches goal
        map[position.x, position.y].isEmpty = true;
        map[position.x, position.y].hasEnemy = false;
        map[position.x, position.y].enemy = null;
        manager.RemoveEnemy(this);
        Destroy(this.gameObject);
    }
    
    protected override void Death() {
        //used when enemy is killed
        manager.AddGold(goldValue);
        map[position.x, position.y].isEmpty = true;
        map[position.x, position.y].hasEnemy = false;
        map[position.x, position.y].enemy = null;
        manager.RemoveEnemy(this);
        Destroy(this.gameObject);
    }
    
    public void Move() {
        int stamina = this.Stamina;
        while (true) {
            if (manager.goals.Contains(this.position)) {
                //enemy reached goal
                manager.SubstractLife(this);
                return;
            }
            
            Vector2Int nextPosition = this.Path[0];
            
            if (map[nextPosition.x, nextPosition.y].isEmpty) {
                //the next position of enemy path is empty
                int xDiff = Mathf.Abs(this.position.x - nextPosition.x);
                int yDiff = Mathf.Abs(this.position.y - nextPosition.y);
                if (xDiff == yDiff && stamina >= 15) {
                    //moving diagonally
                    stamina -= 15;
                }
                else if (stamina >= 10) {
                    //moving vertically/horizontally
                    stamina -= 10;
                }
                else {
                    //not enough stamina to move
                    break;
                }
                
                //clear current position
                map[this.position.x, this.position.y].isEmpty = true;
                map[this.position.x, this.position.y].hasEnemy = false;
                map[this.position.x, this.position.y].enemy = null;
                
                //move to the next position
                this.position = nextPosition;
                movePosition.SetMovePosition(manager.TileCoordinatesToReal(this.position));
                //this.gameObject.transform.position = manager.TileCoordinatesToReal(this.position);
                
                map[this.position.x, this.position.y].isEmpty = false;
                map[this.position.x, this.position.y].hasEnemy = true;
                map[this.position.x, this.position.y].enemy = this;
                
                //remove current position from path
                this.Path.RemoveAt(0);
                
            }
            else {
                //next position is occupied
                break;
            }
        }
        //todo not sure if it should be here or after all enemies finished moving
        this.Attack();
    }

    
    

    public override void ResetAfterTurn() {
        
    }

    public void SetUp(MapTile[,] map, Vector2Int position,List<Vector2Int> path, GameManager manager) {
        base.SetUp(map, position, manager);
        this.path = new List<Vector2Int>(path);
    }

    protected virtual void Awake() {
        movePosition = GetComponent<MovePosition>();
    }

   
}
