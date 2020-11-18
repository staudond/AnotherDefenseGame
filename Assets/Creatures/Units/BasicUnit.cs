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

    void Awake() {
        canMove = true;
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

        
        if (target != null) {
            target.TakeDmg(damage);
            return true;
        }

        return false;
    }

    public override void SetUp(MapTile[,] map, Vector2Int position, GameManager manager) {
        base.SetUp(map, position, manager);
        canMove = true;
    }

    public override void ResetAfterTurn() {
        canMove = true;
    }

    protected override void Death() {
        map[position.x, position.y].isEmpty = true;
        map[position.x, position.y].unit = null;
        map[position.x, position.y].hasUnit = false;
        manager.RemoveUnit(this);
        Destroy(this.gameObject);
    }
}
