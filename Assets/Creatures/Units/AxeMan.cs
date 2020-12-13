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


    }
}