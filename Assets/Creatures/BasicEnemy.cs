using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class BasicEnemy: BasicCreature
{
    public const int stamina = 15;
    private List<Vector2Int> path;

    public BasicEnemy(Vector2Int pos) : base(pos)
    {
    }
    void move()
    {
        
    }
}
