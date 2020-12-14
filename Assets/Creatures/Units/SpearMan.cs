using System;
using Creatures.Enemies;
using UnityEngine;

namespace Creatures.Units {
    public class SpearMan : BasicUnit {
        public SpearMan(Vector2Int pos) : base(pos) { }


        protected override void Awake() {
            base.Awake();
            damage = UnitProperties.SpearManDmg;
            maxHealth = UnitProperties.SpearManHp;
            attackSpeed = UnitProperties.SpearManAttackSpeed;
            attackCooldown = UnitProperties.SpearManAttackCooldown;
            attackRange = UnitProperties.SpearManAttackRange;
            goldValue = UnitProperties.SpearManGoldValue;
            health = maxHealth;
            remainingAttackCooldown = 0;
            canMove = true;
        }

        protected bool AttackInBounds(Vector2Int pos) {
            if (manager.CheckBounds(pos) && map[pos.x, pos.y].hasEnemy) {
                return DoIndividualAttack(map[pos.x, pos.y].enemy);
            }

            return false;
        }

        protected override bool IndividualAttack() {
            //todo enemies in row
            BasicEnemy target = FindAttackTarget();
            bool res = false;
            if (target != null) {
                int xDiff = target.position.x - position.x;
                int yDiff = target.position.y - position.y;
                if (xDiff == 0) {
                    int sign = Math.Sign(yDiff);
                    if (AttackInBounds(position + new Vector2Int(0, sign * 1))) {
                        res = true;
                    }

                    if (AttackInBounds(position + new Vector2Int(0, sign * 2))) {
                        res = true;
                    }
                }
                else if (yDiff == 0) {
                    int sign = Math.Sign(xDiff);
                    if (AttackInBounds(position + new Vector2Int(sign * 1, 0))) {
                        res = true;
                    }

                    if (AttackInBounds(position + new Vector2Int(sign * 2, 0))) {
                        res = true;
                    }
                }
                else {
                    res = DoIndividualAttack(target);
                }
            }

            return res;
        }
    }
}