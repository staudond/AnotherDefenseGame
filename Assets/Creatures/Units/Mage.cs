using System.Collections.Generic;
using Creatures.Enemies;
using UnityEngine;

namespace Creatures.Units {
    public class Mage : BasicUnit {
        public Mage(Vector2Int pos) : base(pos) { }
        protected int splashRange;
        protected int splashDamage;

        protected override void Awake() {
            base.Awake();
            damage = UnitProperties.MageDmg;
            maxHealth = UnitProperties.MageHp;
            attackSpeed = UnitProperties.MageAttackSpeed;
            attackCooldown = UnitProperties.MageAttackCooldown;
            attackRange = UnitProperties.MageAttackRange;
            goldValue = UnitProperties.MageGoldValue;
            splashRange = UnitProperties.MageSplashRange;
            splashDamage = UnitProperties.MageSplashDamage;
            health = maxHealth;
            remainingAttackCooldown = 0;
            canMove = true;
        }

        protected override bool IndividualAttack() {
            BasicEnemy target = FindAttackTarget();
            bool res = DoIndividualAttack(target);
            if (res) {
                List<Vector2Int> splashTargets = GameManager.RangeVectorsToPositions(target.position,
                    GameManager.AttackRangeToRangeVectors(splashRange));
                foreach (var splashTarget in splashTargets) {
                    if (manager.CheckBounds(splashTarget) && map[splashTarget.x, splashTarget.y].hasEnemy) {
                        map[splashTarget.x, splashTarget.y].enemy.TakeDmg(splashDamage);
                    }
                }
            }
            return res;
        }
    }
}