using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapTile {
    public bool isRoad = true;
    public bool isEmpty = true;
    public bool isSpawn = false;
}

public class GameManager : MonoBehaviour {
    private int mapWidth;

    private int mapHeight;

    //public Tilemap backround;
    private Tilemap roads;
    private Tilemap SpawnPositions;
    private Tilemap GoalPositions;
    
    private List<Vector2Int> spawns;
    private List<Vector2Int> goals;
    private Dictionary<Vector2Int, List<Vector2Int>> pathsFromSpawn;
    
    private MapTile[,] map;
    
    private List<BasicEnemy> enemies;
    private List<BasicUnit> units;
    
    public GameObject obj;
    
    private Camera cam;
    
    private float tileOffset;
    private Vector3 mapOffset;
    
    private bool isPlayersTurn = true;
    private bool isPaused = false;
    private bool isGameOver = false;
    
    private GameObject endTurnButton;

    private BasicUnit selectedUnit = null;
    private GameObject selectedSpawnUnit;
    

    private Pathfinding pathFinding;

    private float tileSize;

    private GameObject highlightingTile;
    private List<GameObject> highlightingTiles;

    private List<Vector2Int> highlightedPositions;

    private void Awake() {
        roads =  GameObject.Find("Road").GetComponent<Tilemap>();
        SpawnPositions = GameObject.Find("SpawnPositions").GetComponent<Tilemap>();
        GoalPositions = GameObject.Find("GoalPositions").GetComponent<Tilemap>();
        endTurnButton = GameObject.FindWithTag("EndTurnButton");
        highlightingTile = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/HighlighTile.prefab");
    }

    // Start is called before the first frame update
    void Start() {
        units = new List<BasicUnit>();
        enemies = new List<BasicEnemy>();
        highlightingTiles = new List<GameObject>();
        highlightedPositions = new List<Vector2Int>();
        Vector3Int sizes = roads.size;
        mapWidth = sizes.x;
        mapHeight = sizes.y;
        tileSize = roads.cellSize.x;
        //Debug.Log(size);
        cam = Camera.main;
        tileOffset = tileSize / 2f;
        mapOffset = roads.transform.position;
        InitializeMap();
        //Debug.Log(backround.size);
        //Debug.Log(roads.size);
        selectedSpawnUnit = obj;
    }

    void Update() {
        
        if (isPlayersTurn) {
            if (Input.GetMouseButtonDown(0)) {
                
                Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null) {
                    if (hit.collider.gameObject.GetComponent(typeof(Tilemap)) == roads) {
                        if (selectedSpawnUnit != null) {
                            SpawnUnit(cam.ScreenToWorldPoint(Input.mousePosition));
                            selectedSpawnUnit = null;
                        }
                        else if (selectedUnit != null) {
                            MoveUnit(cam.ScreenToWorldPoint(Input.mousePosition));
                            selectedUnit = null;
                        }
                    }
                    else{
                        BasicUnit temp = hit.collider.gameObject.GetComponent(typeof(BasicUnit)) as BasicUnit;
                        if (temp != null) {
                            Debug.Log("selected");
                            selectedUnit = temp;
                            if (temp.canMove) {
                                Debug.Log("can move");
                                HighlightTiles(RangeVectorsToPositions(temp.position,
                                    RangeToRangeVectors(UnitProperties.UnitMovementRange)));
                                return;
                            }

                        }
                    }
                } 
                StopHighlighting();
                selectedUnit = null;
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                selectedUnit = null;
                StopHighlighting();
            }
        }
        else {
            EnemyTurn();
        }
    }

    void HighlightTiles(List<Vector2Int> positions) {
        StopHighlighting();
        foreach (var tilePosition in positions) {
            if (CheckBounds(tilePosition)) {
                if (map[tilePosition.x, tilePosition.y].isRoad && !map[tilePosition.x, tilePosition.y].isSpawn &&
                    map[tilePosition.x, tilePosition.y].isEmpty) {
                    highlightedPositions.Add(tilePosition);
                    Vector3 position = TileCoordinatesToReal(tilePosition);
                    highlightingTiles.Add(Instantiate(highlightingTile, position, Quaternion.identity));
                }
            }
        }
    }

    bool CheckBounds(Vector2Int position) {

        return (position.x >= 0 && position.x < mapWidth && position.y >= 0 && position.y < mapHeight);
    }
    void StopHighlighting() {
        foreach (var tile in highlightingTiles) {
            Destroy(tile);
        }
        
        highlightedPositions.Clear();
        highlightingTiles.Clear();
        
    }

    void MoveUnit(Vector3 currentPosition) {
        if (selectedUnit.canMove) {
            Vector2Int tilePosition = RealCoordinatesToNearestTile(currentPosition);
            Vector3 realPosition = GetNearestPositionOnGrid(currentPosition);
            foreach (var possiblePosition in highlightedPositions) {
                if (tilePosition == possiblePosition && map[tilePosition.x, tilePosition.y].isEmpty &&
                    map[tilePosition.x, tilePosition.y].isRoad && !map[tilePosition.x, tilePosition.y].isSpawn) {
                    map[selectedUnit.position.x, selectedUnit.position.y].isEmpty = true;
                    selectedUnit.position = tilePosition;
                    selectedUnit.gameObject.transform.position = realPosition;
                    
                    map[tilePosition.x, tilePosition.y].isEmpty = false;
                    selectedUnit.canMove = false;
                }
            }
        }
        
    }

    // void MoveUnit(Vector3 position) {
    //     if (selectedUnit.canMove) {
    //
    //
    //         Vector2Int tilePosition = RealCoordinatesToNearestTile(position);
    //         Vector3 realPosition = GetNearestPositionOnGrid(position);
    //         List<Vector2Int> possiblePositions = RangeVectorsToPositions(selectedUnit.position,
    //             RangeToRangeVectors(UnitProperties.UnitMovementRange));
    //         foreach (Vector2Int possiblePosition in possiblePositions) {
    //             if (tilePosition == possiblePosition && map[tilePosition.x, tilePosition.y].isEmpty && 
    //                 map[tilePosition.x, tilePosition.y].isRoad && !map[tilePosition.x, tilePosition.y].isSpawn ) {
    //                 selectedUnit.position = tilePosition;
    //                 selectedUnit.gameObject.transform.position = realPosition;
    //                 map[selectedUnit.position.x, selectedUnit.position.y].isEmpty = true;
    //                 map[tilePosition.x, tilePosition.y].isEmpty = false;
    //                 selectedUnit.canMove = false;
    //             }
    //         }
    //         
    //     }
    //     
    //     StopHighlighting();
    // }
    
    void EnemyTurn() {
        isPlayersTurn = true;
        endTurnButton.SetActive(true);
        Debug.Log("Enemy turn ended");
    }

    void InitializeMap() {
        pathsFromSpawn = new Dictionary<Vector2Int, List<Vector2Int>>();
        map = new MapTile[mapWidth, mapHeight];
        spawns = new List<Vector2Int>();
        goals = new List<Vector2Int>();

        for (int i = 0; i < mapWidth; i++) {
            for (int j = 0; j < mapHeight; j++) {
                Vector3 pos = TileCoordinatesToReal(new Vector2Int(i, j));
                Vector3Int position = new Vector3Int((int) (pos.x - tileOffset), (int) (pos.y - tileOffset), 0);
                // Debug.Log(pos+" real pos");
                //  Debug.Log(i+"  "+j+" ffff");
                // Debug.Log(position+" moje");
                // Debug.Log(roads.WorldToCell(pos));
                map[i, j] = new MapTile();

                map[i, j].isRoad = roads.HasTile(position);
                //Debug.Log(SpawnPositions.HasTile(position));
                if (SpawnPositions.HasTile(position)) {
                    //Debug.Log("spwan");
                    spawns.Add(new Vector2Int(i, j));
                    map[i, j].isSpawn = true;
                }

                if (GoalPositions.HasTile(position)) {
                    goals.Add(new Vector2Int(i, j));
                }
            }
        }

        pathFinding = new Pathfinding(map);

        foreach (Vector2Int spawn in spawns) {
            List<Vector2Int> path = pathFinding.VectorFindPath(spawn, goals);
            pathsFromSpawn.Add(spawn, path);
        }
    }

    Vector3 GetNearestPositionOnGrid(Vector3 position) {
        return TileCoordinatesToReal(RealCoordinatesToNearestTile(position));
        ;
    }

    Vector3 TileCoordinatesToReal(Vector2Int tileCoordinates) {
        tileCoordinates.y = (tileCoordinates.y - mapHeight + 1) * (-1); //move [0,0] back to bottom left corner

        tileCoordinates.x -= mapWidth / 2; // coordinates [0,0] are in middle of tile map, so we need to get rid of the negative coordinates
        tileCoordinates.y -= mapHeight / 2; // to use them in 2D array map

        //Unity uses center as position, not left corner, so to compensate offset(half of tile size) is needed
        Vector3 coordinates = new Vector3((tileCoordinates.x * tileSize) + tileOffset,
            (tileCoordinates.y * tileSize) + tileOffset,
            0);
        coordinates += mapOffset;

        return coordinates;
    }

    Vector2Int RealCoordinatesToNearestTile(Vector3 realCoordinates) {
        //Unity uses center as position, not left corner, so to compensate offset(half of tile size) is needed
        realCoordinates -= mapOffset;
        Vector2Int coordinates = new Vector2Int(Mathf.RoundToInt((realCoordinates.x - tileOffset) / tileSize),
            Mathf.RoundToInt((realCoordinates.y - tileOffset) / tileSize));

        coordinates.x +=
            mapWidth / 2; // coordinates [0,0] are in middle of tile map, so we need to get rid of the negative coordinates
        coordinates.y += mapHeight / 2; // to use them in 2D array map, now [0,0] is in bottom left corner
        coordinates.y = (coordinates.y - mapHeight + 1) * (-1); //this moves [0,0] to upper right corner


        return coordinates;
    }

    void SpawnUnit(Vector3 position) {
        Vector2Int tilePosition = RealCoordinatesToNearestTile(position);
        Vector3 finPosition = GetNearestPositionOnGrid(position);

        if (!(finPosition.x < ((-mapWidth * tileSize / 2) + roads.transform.position.x) ||
              finPosition.x > ((mapWidth * tileSize / 2) + roads.transform.position.x) ||
              finPosition.y < ((-mapHeight * tileSize / 2) + roads.transform.position.y) ||
              finPosition.y > ((mapHeight * tileSize / 2) + roads.transform.position.y))) {
            if (map[tilePosition.x, tilePosition.y].isEmpty && !map[tilePosition.x, tilePosition.y].isSpawn ) {
                GameObject unitObject = Instantiate(obj, finPosition, Quaternion.identity);
                BasicUnit unit = unitObject.GetComponent("BasicUnit") as BasicUnit;
                map[tilePosition.x, tilePosition.y].isEmpty = false;
                if (unit != null) {
                    unit.SetUp(map, tilePosition);
                    units.Add(unit);
                }
            }
        }
    }

    public void EndTurn() {
        isPlayersTurn = !isPlayersTurn;
        endTurnButton.SetActive(false);
        
        foreach (var unit in units) {
            unit.ResetAfterTurn();
        }
        foreach (var enemy in enemies) {
            enemy.ResetAfterTurn();
        }

        Debug.Log("turn ended");
    }

    public List<Vector2Int> RangeToRangeVectors(int range) {
        List<Vector2Int> vectors = new List<Vector2Int>();
        for (int i = -range; i <= range; i++) {
            for (int j = -range; j <= range; j++) {
                if(i == 0 && j == 0 )
                    continue;
                vectors.Add( new Vector2Int(i,j));
            }
        }
        
        return vectors;
    }

    public List<Vector2Int> RangeVectorsToPositions (Vector2Int pos,List<Vector2Int> vectors) {
        List<Vector2Int> positions = new List<Vector2Int>();
        foreach (Vector2Int vector in vectors) {
            positions.Add(pos+vector);
        }

        return positions;
    }
}