using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapTile
{
    public bool road = true;  
    public bool empty = true;
}
public class GameManager : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public Tilemap tilemap;
    private MapTile[,] map;
    private Queue<BasicEnemy> enemies;
    private Queue<BasicUnit> units;
    public GameObject obj;
    private Camera cam;
    public float offset = 0.5f;

    private float size;
    // Start is called before the first frame update
    void Start()
    {
        map = new MapTile[mapWidth, mapHeight];
        size = tilemap.cellSize.x;
        Debug.Log(size);
        cam = Camera.main;
        // map = new CreatureTile[mapWidth,mapHeight];
        // foreach (CreatureTile x in map )
        // {
        //     
        // }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            SpawnUnit(cam.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log(Input.mousePosition);
        }
    }


    void InitializeMap()
    {
        
    }

    Vector3 getNearestPositionOnGrid(Vector3 position)
    {
        position -= tilemap.transform.position;
        Debug.Log(tilemap.transform.position);
        int x = Mathf.RoundToInt((position.x-offset) / size);
        int y = Mathf.RoundToInt((position.y-offset) / size);
        Vector3 result = new Vector3((float) (x*size)+offset,(float) (y*size)+offset,0);

        result += tilemap.transform.position;
        return result;
    }
    void SpawnUnit(Vector3 position)
    {
        
        Vector3 finPosition = getNearestPositionOnGrid(position);
        if (!(finPosition.x < ((-mapWidth * size / 2)+tilemap.transform.position.x) || finPosition.x > ((mapWidth * size / 2)+tilemap.transform.position.x) ||
              finPosition.y < ((-mapHeight * size / 2)+tilemap.transform.position.y) || finPosition.y > ((mapHeight * size / 2)+tilemap.transform.position.y)))
        {
            Instantiate(obj,finPosition,Quaternion.identity);
        }
        
    }
    
    
}
