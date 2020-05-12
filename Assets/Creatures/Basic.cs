using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// #if UNITY_EDITOR
// using UnityEditor;
// #endif
public abstract class BasicCreature : MonoBehaviour
{
    public Vector2Int position;
    
    
    public const int maxHealth = 0;
    public const int damage = 0;
    public const int speed = 1;
    private int health;

    public BasicCreature(Vector2Int pos)
    {
        position = pos;
        health = maxHealth;
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
    
    // protected Vector3Int move(Vector2Int position)
    // {
    //     //promyslet jak prase            
    // }
    void Start()
    {
        //spawn(new Vector3(0.5f,0.5f,0));
    }
}
