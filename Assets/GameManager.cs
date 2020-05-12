using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapTile
{
    public bool isRoad = true;  
    public bool isEmpty = true;
}
public class GameManager : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public Tilemap roads;
    public Tilemap SpawnPositions;
    public Tilemap GoalPositions;
    private List<Vector2Int> spawns;
    private List<Vector2Int> goals;
    private MapTile[,] map;
    private List<BasicEnemy> enemies;
    private List<BasicUnit> units;
    public GameObject obj;
    private Camera cam;
    private float tileOffset;
    private Vector3 mapOffset;
    private bool isPlayersTurn = true;
    public GameObject endTurnButton;
    private Dictionary<Vector2Int, List<Vector2Int>> pathsFromSpawn;

    private Pathfinding pathFinding;

    private float size;
    // Start is called before the first frame update
    void Start()
    {
        
        size = roads.cellSize.x;
        //Debug.Log(size);
        cam = Camera.main;
        tileOffset = size / 2f;
        mapOffset = roads.transform.position;
        InitializeMap();
        // map = new CreatureTile[mapWidth,mapHeight];
        // foreach (CreatureTile x in map )
        // {
        //     
        // }
    }
    
    void Update()
    {
        if (isPlayersTurn)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.GetComponent(typeof(Tilemap)) == roads)
                    {
                        SpawnUnit(cam.ScreenToWorldPoint(Input.mousePosition));
                    }
                }
            }
        }
        else
        {
            EnemyTurn();
        }
    }

    void EnemyTurn()
    {
        
    }

    void InitializeMap()
    {
        units = new List<BasicUnit>();
        enemies = new List<BasicEnemy>();
        pathsFromSpawn = new Dictionary<Vector2Int, List<Vector2Int>>();
        map = new MapTile[mapWidth, mapHeight];
        spawns = new List<Vector2Int>();
        goals = new List<Vector2Int>();
        
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                Vector3 pos = TileCoordinatesToReal(new Vector2Int(i,j));
                Vector3Int position = new Vector3Int((int) (pos.x -tileOffset), (int) (pos.y - tileOffset), 0);
                // Debug.Log(pos+" real pos");
                //  Debug.Log(i+"  "+j+" ffff");
                // Debug.Log(position+" moje");
                // Debug.Log(roads.WorldToCell(pos));
                map[i,j] = new MapTile();
                
                map[i, j].isRoad = roads.HasTile(position);
                //Debug.Log(SpawnPositions.HasTile(position));
                if (SpawnPositions.HasTile(position))
                {
                    //Debug.Log("spwan");
                    spawns.Add(new Vector2Int(i,j));
                }
                if (GoalPositions.HasTile(position))
                {
                    goals.Add(new Vector2Int(i,j));
                }
            }
        }
        
        pathFinding = new Pathfinding(mapWidth,mapHeight,map);
        
        foreach (Vector2Int spawn in spawns)
        {
            List<Vector2Int> path = pathFinding.VectorFindPath(spawn, goals);
            pathsFromSpawn.Add(spawn,path);
        }
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
        
        Vector2Int tilePosition = RealCoordinatesToNearestTile(position);
        Vector3 finPosition = GetNearestPositionOnGrid(position);
        
        if (!(finPosition.x < ((-mapWidth * size / 2)+roads.transform.position.x) || finPosition.x > ((mapWidth * size / 2)+roads.transform.position.x) ||
              finPosition.y < ((-mapHeight * size / 2)+roads.transform.position.y) || finPosition.y > ((mapHeight * size / 2)+roads.transform.position.y)))
        {
            if (map[tilePosition.x, tilePosition.y].isEmpty)
            {
                GameObject unitObject = Instantiate(obj, finPosition, Quaternion.identity);
                BasicUnit unit  = unitObject.GetComponent("BasicUnit") as BasicUnit;
                //Instantiate(obj, finPosition, Quaternion.identity);
                map[tilePosition.x, tilePosition.y].isEmpty = false;
                unit.position = tilePosition;
                units.Add(unit);
            }
        }
    }

    public void EndTurn()
    {
        isPlayersTurn = !isPlayersTurn;
        endTurnButton.SetActive(false);
    }
    
}
