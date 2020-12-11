using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class BasicUnit : BasicCreature
{
    
    protected bool canMove;
    

    public bool CanMove {
        get => canMove;
        set => canMove = value;
    }

    protected virtual void Awake() {
        canMove = true;
        //movePosition = GetComponent<MovePosition>();
        //goldValue = 10;
    }
    public BasicUnit(Vector2Int pos) : base(pos) {
        canMove = true;
    }
    
    
    protected override bool IndividualAttack() {
        
        List<Vector2Int> attackPositions = GameManager.RangeVectorsToPositions(position,GameManager.AttackRangeToRangeVectors(AttackRange));
        BasicEnemy target = null;
        int targetDistance = int.MaxValue;
        
        foreach (Vector2Int pos in attackPositions) {
            if (manager.CheckBounds(pos) && map[pos.x, pos.y].hasEnemy) {
                int distance = CalculateDistance(pos);
                if ( distance < targetDistance) {
                    target = map[pos.x, pos.y].enemy;
                    targetDistance = distance;
                }else if (distance == targetDistance &&  target.Health > map[pos.x, pos.y].enemy.Health) {
                    target = map[pos.x, pos.y].enemy;
                }
            }
        }
        
        return DoIndividualAttack(target);
        
    }

    public override void SetUp(MapTile[,] map, Vector2Int position, GameManager manager) {
        base.SetUp(map, position, manager);
        canMove = true;
    }

    public override void ResetAfterTurn() {
        canMove = true;
    }
    
    public void Move(Vector3 targetPosition) {
        if (this.CanMove) {
            //unit didn't move this round
            Vector2Int tileTargetPosition = manager.RealCoordinatesToNearestTile(targetPosition);
            Vector3 realTargetPosition = manager.GetNearestPositionOnGrid(targetPosition);

            if (manager.highlightedPositions.Contains(tileTargetPosition) &&
                map[tileTargetPosition.x, tileTargetPosition.y].isEmpty &&
                map[tileTargetPosition.x, tileTargetPosition.y].isRoad &&
                !map[tileTargetPosition.x, tileTargetPosition.y].isSpawn) {
                
                map[this.position.x, this.position.y].isEmpty = true;
                map[this.position.x, this.position.y].hasUnit = false;
                map[this.position.x, this.position.y].unit = null;
                this.position = tileTargetPosition;
                if (!manager.betweenPhase) {
                    //game is not in phase between waves
                    //movePosition.SetMovePosition(realTargetPosition);
                    StartCoroutine(Movement(realTargetPosition));
                    //this.gameObject.transform.position = realTargetPosition;
                    this.CanMove = false;
                }
                else {
                    //game is in phase between games
                    
                    this.gameObject.transform.position = realTargetPosition;
                    //movePosition.SetMovePosition(realTargetPosition);
                }
                
                

                map[tileTargetPosition.x, tileTargetPosition.y].isEmpty = false;
                map[tileTargetPosition.x, tileTargetPosition.y].hasUnit = true;
                map[tileTargetPosition.x, tileTargetPosition.y].unit = this;
               
            }
        }
    }

    

    protected override void Death() {
        map[position.x, position.y].isEmpty = true;
        map[position.x, position.y].unit = null;
        map[position.x, position.y].hasUnit = false;
        manager.RemoveUnit(this);
        Destroy(this.gameObject);
    }
}
