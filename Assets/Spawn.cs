using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawn : MonoBehaviour
{

    public Tile enemy;

    public Tilemap map;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(enemy);
        map.SetTile(new Vector3Int(16,0,0), enemy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
