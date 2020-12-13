using UnityEngine;

namespace Creatures.Enemies {
    public class Wolf : BasicEnemy {
        public Wolf(Vector2Int pos) : base(pos) { }

        protected override void Awake() {
            base.Awake();
            damage = EnemyProperties.WolfDmg;
            maxHealth = EnemyProperties.WolfHp;
            attackSpeed = EnemyProperties.WolfAttackSpeed;
            stamina = EnemyProperties.WolfStamina;
            goldValue = EnemyProperties.WolfGoldValue;
            attackCooldown = EnemyProperties.WolfAttackCooldown;
            attackRange = EnemyProperties.WolfAttackRange;
            health = maxHealth;
            remainingAttackCooldown = 0;
        }


    }
}
