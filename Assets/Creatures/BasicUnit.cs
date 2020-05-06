using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BasicUnit : BasicCreature
{
    
    
    
// #if UNITY_EDITOR
// // The following is a helper that adds a menu item to create a UnitTile Asset
//     [MenuItem("Assets/Create/UnitTile")]
//     public static void CreateUnitTile()
//     {
//         string path = EditorUtility.SaveFilePanelInProject("Save Unit Tile", "New Unit Tile", "Asset", "Save Unit Tile", "Assets");
//         if (path == "")
//             return;
//         AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Unit>(), path);
//     }
// #endif    
}
