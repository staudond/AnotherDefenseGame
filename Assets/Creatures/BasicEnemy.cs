using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class BasicEnemy: BasicCreature
{
    
    
// #if UNITY_EDITOR
// // The following is a helper that adds a menu item to create a EnemyTile Asset
//     [MenuItem("Assets/Create/EnemyTile")]
//     public static void CreateEnemyTile()
//     {
//         string path = EditorUtility.SaveFilePanelInProject("Save Enemy Tile", "New Enemy Tile", "Asset", "Save Enemy Tile", "Assets");
//         if (path == "")
//             return;
//         AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Enemy>(), path);
//     }
// #endif    
}
