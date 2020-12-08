using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : BasicEnemy
{
    public Goblin(Vector2Int pos) : base(pos) { }

    protected override void  Awake() {
        base.Awake();
        damage = EnemyProperties.GoblinDmg;
        maxHealth = EnemyProperties.GoblinHp;
        attackSpeed = EnemyProperties.GoblinAttackSpeed;
        stamina = EnemyProperties.GoblinStamina;
        goldValue = EnemyProperties.GoblinGoldValue;
        attackCooldown = EnemyProperties.GoblinAttackCooldown;
        attackRange = EnemyProperties.GoblinAttackRange;
        health = maxHealth;
        remainingAttackCooldown = 0;
    }

    
}
