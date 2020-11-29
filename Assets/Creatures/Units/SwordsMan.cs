using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsMan : BasicUnit {
    public SwordsMan(Vector2Int pos) : base(pos) { }

   
    protected override void Awake() {
        base.Awake();
        damage = UnitProperties.SwordsmanDmg;
        maxHealth = UnitProperties.SwordsmanHp;
        attackSpeed = UnitProperties.SwordsmanAttackSpeed;
        attackCooldown = UnitProperties.SwordsmanAttackCooldown;
        attackRange = UnitProperties.SwordsmanAttackRange;
        goldValue = UnitProperties.SwordsmanGoldValue;
        health = maxHealth;
        remainingAttackCooldown = 0;
        canMove = true;
    }

    
}