using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BasicUnit : BasicCreature
{
    public bool canMove;
    public BasicUnit(Vector2Int pos) : base(pos) {
        canMove = true;
    }
    
    
    protected override bool IndividualAttack() {
        
        List<Vector2Int> attackPositions = GameManager.RangeVectorsToPositions(position,GameManager.RangeToRangeVectors(AttackRange));
        BasicEnemy target = null;
        int targetDistance = int.MaxValue;
        
        foreach (Vector2Int pos in attackPositions) {
            if (map[pos.x, pos.y].hasEnemy) {
                int distance = CalculateDistance(pos);
                if ( distance < targetDistance) {
                    target = map[pos.x, pos.y].enemy;
                    targetDistance = distance;
                }else if (distance == targetDistance && target.Health > map[pos.x, pos.y].enemy.Health) {
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
        goldValue = 100;
    }

    public override void ResetAfterTurn() {
        canMove = true;
    }

    public override void Death() {
        manager.RemoveUnit(this);
        Destroy(this.gameObject);
    }
}
