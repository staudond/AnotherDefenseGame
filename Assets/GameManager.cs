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
    private float tileOffset;
    private Vector3 mapOffset;

    private float size;
    // Start is called before the first frame update
    void Start()
    {
        map = new MapTile[mapWidth, mapHeight];
        size = tilemap.cellSize.x;
        Debug.Log(size);
        cam = Camera.main;
        tileOffset = size / 2f;
        mapOffset = tilemap.transform.position;
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
        }
    }


    void InitializeMap()
    {
        
    }

    Vector3 GetNearestPositionOnGrid(Vector3 position)
    {
        return TileCoordinatesToReal(RealCoordinatesToNearestTile(position));;
    }

    Vector3 TileCoordinatesToReal(Vector2Int tileCoordinates)
    {
        
        
        tileCoordinates.y = (tileCoordinates.y - mapHeight + 1)*(-1); //move [0,0] back to bottom left corner
            
        tileCoordinates.x -= mapWidth/2;     // coordinates [0,0] are in middle of tile map, so we need to get rid of the negative coordinates
        tileCoordinates.y -= mapHeight/2;    // to use them in 2D array map
        
       
        //Unity uses center as position, not left corner, so to compensate offset(half of tile size) is needed
        Vector3 coordinates = new Vector3(  (tileCoordinates.x*size)+tileOffset,
                                            (tileCoordinates.y*size)+tileOffset,
                                            0); 
        coordinates += mapOffset;
        return coordinates;
    }

    Vector2Int RealCoordinatesToNearestTile(Vector3 realCoordinates)
    {   
        //Unity uses center as position, not left corner, so to compensate offset(half of tile size) is needed
        realCoordinates -= mapOffset;
        Vector2Int coordinates = new Vector2Int(Mathf.RoundToInt((realCoordinates.x-tileOffset)/size),
                                                Mathf.RoundToInt((realCoordinates.y-tileOffset)/size));
        
        coordinates.x += mapWidth / 2;    // coordinates [0,0] are in middle of tile map, so we need to get rid of the negative coordinates
        coordinates.y += mapHeight / 2;   // to use them in 2D array map, now [0,0] is in bottom left corner
        coordinates.y = (coordinates.y - mapHeight + 1)*(-1);  //this moves [0,0] to upper right corner
   

        return coordinates;
    }
    void SpawnUnit(Vector3 position)
    {
        
        Vector3 finPosition = GetNearestPositionOnGrid(position);
        if (!(finPosition.x < ((-mapWidth * size / 2)+tilemap.transform.position.x) || finPosition.x > ((mapWidth * size / 2)+tilemap.transform.position.x) ||
              finPosition.y < ((-mapHeight * size / 2)+tilemap.transform.position.y) || finPosition.y > ((mapHeight * size / 2)+tilemap.transform.position.y)))
        {
            Instantiate(obj,finPosition,Quaternion.identity);
        }
        
    }
    
    
}
