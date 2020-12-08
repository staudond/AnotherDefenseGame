using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Orc : BasicEnemy
{
    public Orc(Vector2Int pos) : base(pos) { }

    protected override void Awake() {
        base.Awake();
        damage = EnemyProperties.OrcDmg;
        maxHealth = EnemyProperties.OrcHp;
        attackSpeed = EnemyProperties.OrcAttackSpeed;
        stamina = EnemyProperties.OrcStamina;
        goldValue = EnemyProperties.OrcGoldValue;
        attackCooldown = EnemyProperties.OrcAttackCooldown;
        attackRange = EnemyProperties.OrcAttackRange;
        health = maxHealth;
        remainingAttackCooldown = 0;
    }

    
}
