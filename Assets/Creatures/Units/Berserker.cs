using UnityEngine;
namespace Creatures.Units {
    public class Berserker :  BasicUnit {
        public Berserker(Vector2Int pos) : base(pos) { }
        

        protected override void Awake() {
            base.Awake();
            damage = UnitProperties.BerserkerDmg;
            maxHealth = UnitProperties.BerserkerHp;
            attackSpeed = UnitProperties.BerserkerAttackSpeed;
            attackCooldown = UnitProperties.BerserkerAttackCooldown;
            attackRange = UnitProperties.BerserkerAttackRange;
            goldValue = UnitProperties.BerserkerGoldValue;
            health = maxHealth;
            remainingAttackCooldown = 0;
            canMove = true;
        }


    }
}