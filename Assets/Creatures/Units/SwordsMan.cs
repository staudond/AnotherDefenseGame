using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsMan : BasicUnit
{
    public SwordsMan(Vector2Int pos) : base(pos) {
    }
    // Start is called before the first frame update
    void Awake() {
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
