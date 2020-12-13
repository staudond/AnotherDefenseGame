using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Units {
    public class SwordsMan : BasicUnit {
        public SwordsMan(Vector2Int pos) : base(pos) { }


        protected override void Awake() {
            base.Awake();
            damage = UnitProperties.SwordsManDmg;
            maxHealth = UnitProperties.SwordsManHp;
            attackSpeed = UnitProperties.SwordsManAttackSpeed;
            attackCooldown = UnitProperties.SwordsManAttackCooldown;
            attackRange = UnitProperties.SwordsManAttackRange;
            goldValue = UnitProperties.SwordsManGoldValue;
            health = maxHealth;
            remainingAttackCooldown = 0;
            canMove = true;
        }


    }
}