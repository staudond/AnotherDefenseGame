using UnityEngine;

namespace Creatures.Enemies {
    public class Spider : BasicEnemy {
        public Spider(Vector2Int pos) : base(pos) { }

        protected override void Awake() {
            base.Awake();
            damage = EnemyProperties.SpiderDmg;
            maxHealth = EnemyProperties.SpiderHp;
            attackSpeed = EnemyProperties.SpiderAttackSpeed;
            stamina = EnemyProperties.SpiderStamina;
            goldValue = EnemyProperties.SpiderGoldValue;
            attackCooldown = EnemyProperties.SpiderAttackCooldown;
            attackRange = EnemyProperties.SpiderAttackRange;
            health = maxHealth;
            remainingAttackCooldown = 0;
        }


    }
}
