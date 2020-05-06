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
    public Vector3 position;
    
    public void spawn(Vector3 position)
    {
        this.position = position;
        //Instantiate(gameObject);
        gameObject.transform.position = new Vector3(position.x  ,position.y,position.z);
    }
    
    
    
    public const int maxHealth = 0;
    public const int damage = 0;
    public const int speed = 1;

    
    // protected Vector3Int move(Vector3Int position)
    // {
    //     //promyslet jak prase            
    // }
    void Start()
    {
        //spawn(new Vector3(0.5f,0.5f,0));
    }
}
