using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
// #if UNITY_EDITOR
// using UnityEditor;
// #endif
public abstract class BasicCreature : MonoBehaviour
{
    public event EventHandler OnHealthChanged;
    public Vector2Int position;
    protected MapTile[,] map;

    public int MaxHealth => maxHealth;

    public int Damage => damage;

    public int AttackSpeed => attackSpeed;

    public int AttackCooldown => attackCooldown;

    protected int maxHealth;
    protected int damage ;
    protected  int attackSpeed; //number of attack creature can do at once when attacks
    protected  int attackCooldown ; //number of turns before creature can attack again
    protected int remainingAttackCooldown;
    protected int health;
    protected GameManager manager;
    protected int goldValue = 1;

    public int GoldValue => goldValue;

    public int Health => health;

    
    public int attackRange;

     public int AttackRange => attackRange;

     public virtual void SetUp(MapTile[,] map, Vector2Int position,GameManager manager) {
         
         this.position = position;
        this.map = map;
        this.manager = manager;
        health = maxHealth;
        //canMove = true;
     }

    public bool IsAlive() {
        return health > 0;
    }

    public float GetHealthPercent() {
        return (float)health / maxHealth;
    }

    public void Attack() {
        if (remainingAttackCooldown > 0) {
            remainingAttackCooldown--;
        }
        else {
            bool didAttack = false;
            for (int i = 0; i < attackSpeed; i++) {
                if (IndividualAttack()) {
                    didAttack = true;
                }
                else {
                    break;
                }

            }

            if (didAttack) {
                remainingAttackCooldown = attackCooldown;
            }
        }
    }

    
    
    protected abstract bool IndividualAttack();

    protected void ChangeHealth() {
        if (OnHealthChanged != null) {
            OnHealthChanged(this,EventArgs.Empty);
        }
    }
    
    public virtual void TakeDmg(int dmg) {
        health -= dmg;
        DamagePopUp.Create(gameObject.transform.position+new Vector3(0f,0.7f), dmg);
        ChangeHealth();
        if (health <= 0) {
            Death();
        }
    }

    protected abstract void Death();

    protected int CalculateDistance(Vector2Int target)
    {
        int xDistance = Mathf.Abs(position.x - target.x);
        int yDistance = Mathf.Abs(position.y - target.y);
        int rest = Mathf.Abs(xDistance - yDistance);
        return Mathf.Min(xDistance, yDistance) + rest;
    }

    public abstract void ResetAfterTurn();

    protected BasicCreature(Vector2Int pos)
    {
        position = pos;
        //canMove = true;
        health = maxHealth;
    }
    
    void Awake()
    {
        maxHealth = 10;
        damage = 0;
        attackSpeed = 1;
        attackCooldown = 1;
        attackRange = 2;
        health = maxHealth;
        remainingAttackCooldown = 0;
    }
}
