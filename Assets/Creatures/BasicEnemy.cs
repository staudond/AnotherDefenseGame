﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class BasicEnemy: BasicCreature
{
    public int stamina = 15;
    public List<Vector2Int> path;
    
    

    public BasicEnemy(Vector2Int pos) : base(pos)
    {
    }
    
    protected override bool IndividualAttack() {
        List<Vector2Int> attackPositions = GameManager.RangeVectorsToPositions(position,GameManager.RangeToRangeVectors(AttackRange));
        BasicUnit target = null;
        int targetDistance = int.MaxValue;
        foreach (Vector2Int pos in attackPositions) {
            if (!map[pos.x, pos.y].isEmpty && !map[pos.x, pos.y].hasEnemy) {
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
                if (!map[pos.x, pos.y].isEmpty && !map[pos.x, pos.y].hasEnemy) {
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


        if (target != null) {
            target.TakeDmg(damage);
            return true;
        }

        return false;
    }

    public override void Death() {
        map[position.x, position.y].isEmpty = true;
        map[position.x, position.y].hasEnemy = false;
        map[position.x, position.y].enemy = null;
        manager.RemoveEnemy(this);
        Destroy(this.gameObject);
    }

    public override void TakeDmg(int dmg) {
        health -= dmg;
        if (health <= 0) {
            manager.AddGold(goldValue);
            Death();
        }
        
    }

    public override void ResetAfterTurn() {
        
    }

    public void SetUp(MapTile[,] map, Vector2Int position,List<Vector2Int> path, GameManager manager) {
        base.SetUp(map, position, manager);
        this.path = new List<Vector2Int>(path);
        goldValue = 50;
    }

    void Start() {
        
    }
}
