using Creatures.Enemies;
using UnityEngine;

namespace Creatures.Units {
    public class AxeMan :  BasicUnit {
        public AxeMan(Vector2Int pos) : base(pos) { }
        

        protected override void Awake() {
            base.Awake();
            damage = UnitProperties.AxeManDmg;
            maxHealth = UnitProperties.AxeManHp;
            attackSpeed = UnitProperties.AxeManAttackSpeed;
            attackCooldown = UnitProperties.AxeManAttackCooldown;
            attackRange = UnitProperties.AxeManAttackRange;
            goldValue = UnitProperties.AxeManGoldValue;
            health = maxHealth;
            remainingAttackCooldown = 0;
            canMove = true;
        }

        protected void AttackInBounds(Vector2Int pos) {
            if (manager.CheckBounds(pos) && map[pos.x, pos.y].hasEnemy) {
                DoIndividualAttack(map[pos.x, pos.y].enemy);
            }
        }
        protected override bool IndividualAttack() {
            BasicEnemy target = FindAttackTarget();

            bool res = DoIndividualAttack(target);
            if (res) {
                int xDiff = target.position.x - position.x;
                int yDiff = target.position.y - position.y;
                if (xDiff == 0) {
                    //target is up or down
                    AttackInBounds(target.position + new Vector2Int(1, 0));
                    AttackInBounds(target.position + new Vector2Int(-1, 0));
                }
                else if (yDiff == 0) {
                    //target is right or left
                    AttackInBounds(target.position + new Vector2Int(0, 1));
                    AttackInBounds(target.position + new Vector2Int(0, -1));
                }
                else if (xDiff == 1) {
                    //tqrget is right diagonally
                    AttackInBounds(target.position + new Vector2Int(-1, 0));
                    if (yDiff == 1) {
                        //target is down diagonally
                        AttackInBounds(target.position + new Vector2Int(0, -1));
                    }
                    else if (yDiff == -1) {
                        //target is up diagonally
                        AttackInBounds(target.position + new Vector2Int(0, 1));
                    }
                }
                else if (xDiff == -1) {
                    //target is left diagonally
                    AttackInBounds(target.position + new Vector2Int(1, 0));
                    if (yDiff == 1) {
                        //target is down diagonally
                        AttackInBounds(target.position + new Vector2Int(0, -1));
                    }
                    else if (yDiff == -1) {
                        //target is up diagonally
                        AttackInBounds(target.position + new Vector2Int(0, 1));
                    }
                }
            }

            return res;
        }
    }
}