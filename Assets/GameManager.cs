using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapTile {
    public bool isRoad = true;
    public bool isEmpty = true;
    public bool isSpawn = false;
    public bool hasEnemy = false;
    public BasicEnemy enemy = null;
    public BasicUnit unit = null;
}

public class GameManager : MonoBehaviour {
    private int mapWidth;

    private int mapHeight;

    private Tilemap background;
    private Tilemap roads;
    private Tilemap spawnPositions;
    private Tilemap goalPositions;

    private List<Vector2Int> spawns;
    private List<Vector2Int> goals;
    private Dictionary<Vector2Int, List<Vector2Int>> pathsFromSpawn;

    private MapTile[,] map;
    public List<GameObject> allEnemies;
    public List<GameObject> allUnits;
            
    private List<BasicEnemy> enemies;
    private List<BasicUnit> units;

    public GameObject obj;

    private Camera cam;

    private float tileOffset;
    private Vector3 mapOffset;

    private bool isPlayersTurn = true;
    private static bool isPaused = false;

    public static bool IsPaused => isPaused;

    public static bool IsGameOver => isGameOver;

    private static bool isGameOver = false;
    private bool won = false;

    private GameObject endTurnButton;

    private BasicUnit selectedUnit = null;
    private GameObject selectedSpawnUnit;


    private Pathfinding pathFinding;

    private float tileSize;

    private GameObject highlightingTile;
    private List<GameObject> highlightingTiles;

    private List<Vector2Int> highlightedPositions;
    private Text LivesText; 
    private Text GoldText; 
    
    [SerializeField] private int playerLives = 20;
    
    [SerializeField] private int playerGold = 200;
    private GameObject pauseScreen;
    private GameObject gameOverScreen;
    private GameObject winningScreen;

    public int PlayerGold => playerGold;

    public void AddGold(int gold) {
        playerGold += gold;
    }

    public void RemoveEnemy(BasicEnemy enemy) {
        enemies.Remove(enemy);
    }
    public void RemoveUnit(BasicUnit unit) {
        units.Remove(unit);
    }
    private void Awake() {
        roads = GameObject.Find("Road").GetComponent<Tilemap>();
        spawnPositions = GameObject.Find("SpawnPositions").GetComponent<Tilemap>();
        goalPositions = GameObject.Find("GoalPositions").GetComponent<Tilemap>();
        background = GameObject.Find("Background").GetComponent<Tilemap>();
        endTurnButton = GameObject.FindWithTag("EndTurnButton");
        highlightingTile = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/HighlighTile.prefab");
        LivesText = GameObject.Find("Lives").GetComponent<Text>();
        GoldText = GameObject.Find("Gold").GetComponent<Text>();
        pauseScreen = GameObject.Find("PauseScreen");
        pauseScreen.SetActive(false);
        gameOverScreen = GameObject.Find("GameOverScreen");
        gameOverScreen.SetActive(false);
        winningScreen = GameObject.Find("WinningScreen");
        winningScreen.SetActive(false);
    }

    // Start is called before the first frame update
    void Start() {
        isPaused = false;
        isGameOver = false;
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

        Debug.Log(background.localBounds.max);
        Debug.Log(background.localBounds.min);
        Debug.Log(background.localBounds.center);
        //Debug.Log("Xmax "+background.cellBounds.xMax+" Xmin "+background.cellBounds.xMin);
        //Debug.Log("Ymax "+background.cellBounds.yMax+" Ymin "+background.cellBounds.yMin);
    }

    void Update() {
        
        LivesText.text = "LIVES: " + playerLives;
        GoldText.text = "GOLD: " + playerGold;
        if (playerLives <= 0) {
            isGameOver = true;
            gameOverScreen.SetActive(true);
        }

        if (isGameOver && won) {
            winningScreen.SetActive(true);
        }

        if (!isGameOver) {
            if (isPaused) {
                if (Input.GetKeyDown(KeyCode.P)) {
                    Unpause();

                }
            }
            else if (isPlayersTurn) {
                if (Input.GetKeyDown(KeyCode.P)) {
                    Pause();
                }

                if (Input.GetMouseButtonDown(0)) {

                    Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                    if (hit.collider != null) {
                        if (hit.collider.gameObject.GetComponent(typeof(Tilemap)) == roads) {
                            if (selectedSpawnUnit != null) {
                                if (selectedSpawnUnit.GetComponent<BasicUnit>().GoldValue <= playerGold) {
                                    SpawnUnit(cam.ScreenToWorldPoint(Input.mousePosition));
                                    selectedSpawnUnit = null;
                                }
                            }
                            else if (selectedUnit != null) {
                                MoveUnit(cam.ScreenToWorldPoint(Input.mousePosition));
                                selectedUnit = null;
                            }
                        }
                        else {
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

    void MoveUnit(Vector3 currentPosition) { //todo move to basic unit script
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

    void MoveEnemy(BasicEnemy enemy) {
        int stamina = enemy.stamina;
        while (true) {
            if (goals.Contains(enemy.position)) {
                playerLives--;
                enemy.Death();
                return;
            }
            Vector2Int nextPosition = enemy.path[0];
            
            if (map[nextPosition.x, nextPosition.y].isEmpty) {
                int xDiff = Mathf.Abs(enemy.position.x - nextPosition.x);
                int yDiff = Mathf.Abs(enemy.position.y - nextPosition.y);
                if (xDiff == yDiff && stamina >= 15) {
                    stamina -= 15;
                }
                else if (stamina >= 10) {
                    stamina -= 10;
                }
                else {
                    return;
                }
                
                map[enemy.position.x, enemy.position.y].isEmpty = true;
                map[enemy.position.x, enemy.position.y].hasEnemy = false;
                map[enemy.position.x, enemy.position.y].enemy = null;
                enemy.position = nextPosition;
                enemy.gameObject.transform.position = TileCoordinatesToReal(enemy.position);
                map[enemy.position.x, enemy.position.y].isEmpty = false;
                map[enemy.position.x, enemy.position.y].hasEnemy = true;
                map[enemy.position.x, enemy.position.y].enemy = enemy;
                enemy.path.RemoveAt(0);
            }
            else {
                return;
            }
        }
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
                if (spawnPositions.HasTile(position)) {
                    //Debug.Log("spwan");
                    spawns.Add(new Vector2Int(i, j));
                    map[i, j].isSpawn = true;
                }

                if (goalPositions.HasTile(position)) {
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

        tileCoordinates.x -=
            mapWidth / 2; // coordinates [0,0] are in middle of tile map, so we need to get rid of the negative coordinates
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

    public void SpawnEnemy() {
        List<Vector2Int> tempSpawns = new List<Vector2Int>(spawns);
        Vector2Int spawn = new Vector2Int();
        bool found = false;
        while (tempSpawns.Count != 0) {
            int spawnNumber = Random.Range(0, spawns.Count);
            spawn = spawns[spawnNumber];
            if (!map[spawn.x, spawn.y].isEmpty) {
                tempSpawns.Remove(spawn);
            }
            else {
                found = true;
                break;
            }
        }

        if (!found) {
            return;
        }
        
        Vector3 pos = TileCoordinatesToReal(spawn);
        if (map[spawn.x, spawn.y].isEmpty && map[spawn.x, spawn.y].isRoad) {
            GameObject enemyObject = Instantiate(allEnemies[0], pos, Quaternion.identity);
            BasicEnemy enemy = enemyObject.GetComponent<BasicEnemy>();

            if (enemy != null) {
                map[spawn.x, spawn.y].isEmpty = false;
                map[spawn.x, spawn.y].hasEnemy = true;
                map[spawn.x, spawn.y].enemy = enemy;
                enemy.SetUp(map, spawn, pathsFromSpawn[spawn],this);
                enemies.Add(enemy);
            }
        }
    }

    void SpawnUnit(Vector3 position) {
        Vector2Int tilePosition = RealCoordinatesToNearestTile(position);
        Vector3 finPosition = GetNearestPositionOnGrid(position);

        if (!(finPosition.x < ((-mapWidth * tileSize / 2) + roads.transform.position.x) ||
              finPosition.x > ((mapWidth * tileSize / 2) + roads.transform.position.x) ||
              finPosition.y < ((-mapHeight * tileSize / 2) + roads.transform.position.y) ||
              finPosition.y > ((mapHeight * tileSize / 2) + roads.transform.position.y))) {
            if (map[tilePosition.x, tilePosition.y].isEmpty && !map[tilePosition.x, tilePosition.y].isSpawn) {
                GameObject unitObject = Instantiate(obj, finPosition, Quaternion.identity);
                BasicUnit unit = unitObject.GetComponent<BasicUnit>();
                
                
                if (unit != null) {
                    map[tilePosition.x, tilePosition.y].isEmpty = false;
                    map[tilePosition.x, tilePosition.y].hasEnemy = false;
                    map[tilePosition.x, tilePosition.y].unit = unit;
                    unit.SetUp(map, tilePosition,this);
                    playerGold -= unit.GoldValue;
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
            unit.Attack();
        }

        
        //Debug.Log("turn ended");
    }
    
    void EnemyTurn() {
        for (int i = enemies.Count-1 ; i >=0; --i) {
            enemies[i].ResetAfterTurn();
            MoveEnemy(enemies[i]);
        }
        // foreach (var enemy in enemies) {
        //     enemy.ResetAfterTurn();
        //     MoveEnemy(enemy);
        //     //Debug.Log(enemy.Health);
        // }
        
        //SpawnEnemy();
        
        isPlayersTurn = true;
        endTurnButton.SetActive(true);
        
        //Debug.Log("Enemy turn ended");
    }

    public static List<Vector2Int> RangeToRangeVectors(int range) {
        List<Vector2Int> vectors = new List<Vector2Int>();
        for (int i = -range; i <= range; i++) {
            for (int j = -range; j <= range; j++) {
                if (i == 0 && j == 0)
                    continue;
                vectors.Add(new Vector2Int(i, j));
            }
        }

        return vectors;
    }

    public static List<Vector2Int> RangeVectorsToPositions(Vector2Int pos, List<Vector2Int> vectors) {
        List<Vector2Int> positions = new List<Vector2Int>();
        foreach (Vector2Int vector in vectors) {
            positions.Add(pos + vector);
        }

        return positions;
    }

    public static List<Vector2Int> AttackRangeToRangeVectors(int range) {
        List<Vector2Int> vectors = new List<Vector2Int>();
        for (int y = -range; y <= range; y++) {
            for (int x = -(range - Mathf.Abs(y)); x <= range - Mathf.Abs(y); x++) {
                if (x == 0 && y == 0)
                    continue;
                vectors.Add(new Vector2Int(x, y));
            }
        }

        return vectors;
    }

    public void Pause() {
        isPaused = true;
        pauseScreen.SetActive(true);
    }
    public void Unpause() {
        isPaused = false;
        pauseScreen.SetActive(false);
    }
}