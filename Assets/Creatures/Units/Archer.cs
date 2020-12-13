using UnityEngine;

namespace Creatures.Units {
    public class Archer : BasicUnit {
        public Archer(Vector2Int pos) : base(pos) { }


        protected override void Awake() {
            base.Awake();
            damage = UnitProperties.ArcherDmg;
            maxHealth = UnitProperties.ArcherHp;
            attackSpeed = UnitProperties.ArcherAttackSpeed;
            attackCooldown = UnitProperties.ArcherAttackCooldown;
            attackRange = UnitProperties.ArcherAttackRange;
            goldValue = UnitProperties.ArcherGoldValue;
            health = maxHealth;
            remainingAttackCooldown = 0;
            canMove = true;
        }


    }
}