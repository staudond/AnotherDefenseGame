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

        protected override bool IndividualAttack() {
            //todo enemies in row
            return base.IndividualAttack();
        }
    }
}