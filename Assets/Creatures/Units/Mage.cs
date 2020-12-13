using UnityEngine;

namespace Creatures.Units {
    public class Mage : BasicUnit {
        public Mage(Vector2Int pos) : base(pos) { }


        protected override void Awake() {
            base.Awake();
            damage = UnitProperties.MageDmg;
            maxHealth = UnitProperties.MageHp;
            attackSpeed = UnitProperties.MageAttackSpeed;
            attackCooldown = UnitProperties.MageAttackCooldown;
            attackRange = UnitProperties.MageAttackRange;
            goldValue = UnitProperties.MageGoldValue;
            health = maxHealth;
            remainingAttackCooldown = 0;
            canMove = true;
        }

        protected override bool IndividualAttack() {
            //todo splash damage
            return base.IndividualAttack();
        }
    }
}