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
    
    //protected MovePosition movePosition;

    public int MaxHealth => maxHealth;

    public int Damage => damage;

    public int AttackSpeed => attackSpeed;

    public int AttackCooldown => attackCooldown;
    protected List<GameObject> highlightTiles;
    protected int maxHealth;
    protected int damage ;
    protected  int attackSpeed; //number of attack creature can do at once when attacks
    protected  int attackCooldown ; //number of turns before creature can attack again
    protected int remainingAttackCooldown;
    protected int health;
    protected GameManager manager;
    protected int goldValue = 1;
    protected float moveSpeed;
    
    protected Vector3 velocityVector;

    public void SetVelocity(Vector3 velocity) {
        velocityVector = velocity;
    }

    public int GoldValue => goldValue;

    public int Health => health;

    
    public int attackRange;

     public int AttackRange => attackRange;

     public virtual void SetUp(MapTile[,] map, Vector2Int position,GameManager manager) {
         
         this.position = position;
        this.map = map;
        this.manager = manager;
        health = maxHealth;
        moveSpeed = Properties.animMoveSpeed;
        highlightTiles = new List<GameObject>();
        //canMove = true;
     }

    public bool IsAlive() {
        return health > 0;
    }

    public float GetHealthPercent() {
        return (float)health / maxHealth;
    }

    public IEnumerator Attack() {
        if (remainingAttackCooldown > 0) {
            remainingAttackCooldown--;
        }
        else {
            for (int i = 0; i < attackSpeed; i++) {
                if (IndividualAttack()) {
                    remainingAttackCooldown = attackCooldown;
                    if (!GameManager.skip) {
                        yield return new WaitForSeconds(Properties.nextAttackDelay);
                    }

                    foreach (var tile in highlightTiles) {
                        Destroy(tile);
                    }
                    highlightTiles.Clear();
                }
                else{
                    break;
                }

            }
        }
    }

    
    
    protected abstract bool IndividualAttack();
    
    protected bool DoIndividualAttack(BasicCreature target) {
        if (target != null) {
            
            highlightTiles.Add(Instantiate(manager.unitHighlightingTile,gameObject.transform.position,Quaternion.identity));
            highlightTiles.Add(Instantiate(manager.enemyHighlightingTile,target.gameObject.transform.position,Quaternion.identity));
            target.TakeDmg(damage);
            return true;
        }
        return false;
    }
    
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

    protected IEnumerator  Movement(Vector3 movePosition) {
        while (true) {
            Vector3 moveDir = (movePosition - transform.position).normalized;
            if (Vector3.Distance(movePosition, transform.position) < 0.1f) {
                // Stop moving when near target position
                transform.position = movePosition;
                break;
            }

            transform.position += moveDir * moveSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
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
        //movePosition = GetComponent<MovePosition>();
    }

    protected void Update() {
        
    }
}
