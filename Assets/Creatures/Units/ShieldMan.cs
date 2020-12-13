using UnityEngine;

namespace Creatures.Units {
    public class ShieldMan : BasicUnit {
        public ShieldMan(Vector2Int pos) : base(pos) { }


        protected override void Awake() {
            base.Awake();
            damage = UnitProperties.ShieldManDmg;
            maxHealth = UnitProperties.ShieldManHp;
            attackSpeed = UnitProperties.ShieldManAttackSpeed;
            attackCooldown = UnitProperties.ShieldManAttackCooldown;
            attackRange = UnitProperties.ShieldManAttackRange;
            goldValue = UnitProperties.ShieldManGoldValue;
            health = maxHealth;
            remainingAttackCooldown = 0;
            canMove = true;
        }


    }
}