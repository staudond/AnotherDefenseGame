using UnityEngine;

namespace Creatures.Units {
    public class CrossbowMan : BasicUnit {
        public CrossbowMan(Vector2Int pos) : base(pos) { }


        protected override void Awake() {
            base.Awake();
            damage = UnitProperties.CrossbowManDmg;
            maxHealth = UnitProperties.CrossbowManHp;
            attackSpeed = UnitProperties.CrossbowManAttackSpeed;
            attackCooldown = UnitProperties.CrossbowManAttackCooldown;
            attackRange = UnitProperties.CrossbowManAttackRange;
            goldValue = UnitProperties.CrossbowManGoldValue;
            health = maxHealth;
            remainingAttackCooldown = 0;
            canMove = true;
        }


    }
}