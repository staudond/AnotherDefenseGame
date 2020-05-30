using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : BasicEnemy
{
    public Orc(Vector2Int pos) : base(pos) { }

    void Awake() {
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
