using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using Creatures.Units;
using Creatures.Enemies;
public enum Units{None = 0,SpearMan = 1,SwordsMan = 2,Archer = 3, AxeMan = 4, 
    CrossbowMan = 5, Berserker = 6, ShieldMan = 7,Mage = 8}
public enum Enemies{None = 0, Goblin = 1,Orc = 2, Spider = 3, Wolf = 4}



public class MapTile {
    public bool isRoad = true;
    public bool isEmpty = true;
    public bool isSpawn = false;
    public bool hasEnemy = false;
    public bool hasUnit = false;
    public BasicEnemy enemy = null;
    public BasicUnit unit = null;
}

public class GameManager : MonoBehaviour {
    
    //if true, animations will be skipped that turn
    public static bool skip;
    
    private int mapWidth;

    private int mapHeight;


    private int currentDelayBetweenPartsOfWaves;
    private Tilemap background;
    private Tilemap roads;
    private Tilemap spawnPositions;
    private Tilemap goalPositions;

    private List<Vector2Int> spawns;
    public List<Vector2Int> goals { get; private set; }
    
    private Dictionary<Vector2Int, List<Vector2Int>> pathsFromSpawn;
    
    private MapTile[,] map;
    private List<GameObject> allEnemies;
    private List<GameObject> allUnits;
    private List<int> unitValues;        
    private List<BasicEnemy> enemies;
    private List<BasicUnit> units;

    private Camera cam;

    private float tileOffset;
    private Vector3 mapOffset;

    private bool isPlayersTurn = true;
    private static bool isPaused = false;

    public bool betweenPhase { get; private set; } = false;
    
    private bool endOfWave = false;

    public static bool IsPaused => isPaused;

    public static bool IsGameOver => isGameOver;

    private static bool isGameOver = false;
    private bool won = false;

    private GameObject endTurnButton;
    private GameObject skipButton; 

    private BasicUnit selectedUnit = null;
    private Units selectedSpawnUnit;
   

    private Pathfinding pathFinding;

    private float tileSize;

    private GameObject highlightingTile;
    public GameObject enemyHighlightingTile{ get; private set; }
    public GameObject unitHighlightingTile{ get; private set; }
    private GameObject rangeHighlightingTile;
    
    private List<GameObject> highlightingTiles;

    public List<Vector2Int> highlightedPositions { get; private set; }
    private Text LivesText; 
    private Text GoldText; 
    private Text WaveText;
    private GameObject PhaseText;
    
    [SerializeField] private int playerLives = Properties.playerStartLives;
    
    [SerializeField] private int playerGold = Properties.playerStartGold;
    
    private GameObject pauseScreen;
    private GameObject gameOverScreen;
    private GameObject winningScreen;

    private MapWaves waves;
    private EnemyWave currentWave = null;

    public int PlayerGold => playerGold;

    public void  SubstractLife(BasicEnemy enemy ) {
        playerLives--;
        enemy.Disappear();
        if (playerLives <= 0) {
            isGameOver = true;
            gameOverScreen.SetActive(true);
        }
    }
    
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
        skipButton = GameObject.FindWithTag("SkipButton");
        skipButton.SetActive(false);
        highlightingTile = Resources.Load<GameObject>("Prefabs/HighlightTiles/HighlightTile");
        enemyHighlightingTile = Resources.Load<GameObject>("Prefabs/HighlightTiles/EnemyHighlightTile");
        unitHighlightingTile = Resources.Load<GameObject>("Prefabs/HighlightTiles/UnitHighlightTile");
        rangeHighlightingTile = Resources.Load<GameObject>("Prefabs/HighlightTiles/RangeHighlightTile");
        
        
        LivesText = GameObject.Find("Lives").GetComponent<Text>();
        GoldText = GameObject.Find("PlayerGold").GetComponent<Text>();
        WaveText = GameObject.Find("Wave").GetComponent<Text>();
        PhaseText = GameObject.Find("Phase");
        PhaseText.SetActive(false);
        pauseScreen = GameObject.Find("PauseScreen");
        pauseScreen.SetActive(false);
        gameOverScreen = GameObject.Find("GameOverScreen");
        gameOverScreen.SetActive(false);
        winningScreen = GameObject.Find("WinningScreen");
        winningScreen.SetActive(false);
    }

    // Start is called before the first frame update
    void Start() {
        //todo change
        waves = new TestMapWaves();
        
        isPaused = false;
        isGameOver = false;
        AddAllUnits();
        AddAllEnemies();
        units = new List<BasicUnit>();
        enemies = new List<BasicEnemy>();
        highlightingTiles = new List<GameObject>();
        highlightedPositions = new List<Vector2Int>();
        Vector3Int sizes = roads.size;
        mapWidth = sizes.x;
        mapHeight = sizes.y;
        tileSize = roads.cellSize.x;
        cam = Camera.main;
        tileOffset = tileSize / 2f;
        mapOffset = roads.transform.position;
        InitializeMap();
       
        
        hightile = new List<GameObject>();
        currentWave = waves.NextWave();
        WaveText.text = "Wave " + currentWave.number;
        skip = false;
    }

    private List<GameObject> hightile;
    void Update() {

        LivesText.text =  playerLives.ToString();
        GoldText.text =  playerGold.ToString();

        if (currentWave == null && waves.Empty() && enemies.Count == 0) {
            isGameOver = true;
            won = true;
        }

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
                
                if (selectedSpawnUnit != Units.None) {
                    HighlightEmptyTiles();
                }
                if (Input.GetKeyDown(KeyCode.Space)) {
                    List<List<Vector2Int>> xs = new List<List<Vector2Int>>();

                    if (selectedUnit != null) {
                        xs.Add(RangeVectorsToPositions(selectedUnit.position,
                            AttackRangeToRangeVectors(selectedUnit.AttackRange)));
                    }
                    else {
                        foreach (var unit in units) {
                            xs.Add(RangeVectorsToPositions(unit.position,
                                AttackRangeToRangeVectors(unit.AttackRange)));
                        }
                    }

                    foreach (var x in xs) {
                        foreach (var pos in x) {
                            if (CheckBounds(pos) && map[pos.x, pos.y].isRoad) {
                                hightile.Add(Instantiate(rangeHighlightingTile, TileCoordinatesToReal(pos),
                                    Quaternion.identity));
                            }
                        }
                    }
                }


                if (Input.GetKeyUp(KeyCode.Space)) {
                    foreach (var tile in hightile) {
                        Destroy(tile);
                    }

                    hightile.Clear();
                }
                
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    selectedUnit = null;
                    selectedSpawnUnit = Units.None;
                    EventSystem.current.SetSelectedGameObject(null);
                    StopHighlighting();
                }

                if (Input.GetMouseButtonDown(0)) {
                    //left mouse click
                    if (!EventSystem.current.IsPointerOverGameObject()) {
                        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                        if (hit.collider != null) {
                            //clicked on object with collider(road or unit)
                            print(RealCoordinatesToNearestTile(cam.ScreenToWorldPoint(Input.mousePosition)));
                            if (hit.collider.gameObject.GetComponent<Tilemap>() == roads) {
                                //clicked on road(empty tile)
                                print("road");
                                if (selectedSpawnUnit != Units.None) {
                                    //there is selected unit to spawn
                                    if (unitValues[(int) selectedSpawnUnit - 1] <= playerGold) {
                                        //player have enough gold 
                                        //GameObject current = EventSystem.current.currentSelectedGameObject;
                                        
                                        SpawnUnit(cam.ScreenToWorldPoint(Input.mousePosition));
                                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                                            EventSystem.current.SetSelectedGameObject(LastActiveButton.lastActiveButton);
                                        }
                                        else {
                                            selectedSpawnUnit = Units.None;
                                            StopHighlighting();
                                        }
                                    }
                                    else {
                                        //not enough gold
                                        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
                                        pos.z = 0;
                                        TextPopUp.Create(pos,"Not Enough Gold",Color.red);
                                        selectedSpawnUnit = Units.None;
                                        StopHighlighting();
                                    }
                                }
                                else if (selectedUnit != null) {
                                    //player has selected unit
                                    selectedUnit.Move(cam.ScreenToWorldPoint(Input.mousePosition));
                                    selectedUnit = null;
                                    StopHighlighting();
                                }
                            }
                            else {
                                //we clicked on unit
                                print("unit");
                                StopHighlighting();
                                BasicUnit temp = hit.collider.gameObject.GetComponent<BasicUnit>();
                                if (temp != null) {
                                    selectedUnit = temp;
                                    selectedSpawnUnit = Units.None;
                                    highlightingTiles.Add(Instantiate(unitHighlightingTile,
                                        selectedUnit.gameObject.transform.position, Quaternion.identity));
                                    if (temp.CanMove) {
                                        HighlightTiles(RangeVectorsToPositions(temp.position,
                                            RangeToRangeVectors(UnitProperties.UnitMovementRange)));
                                    }
                                }
                            }
                        }
                        else {
                            //clicked elsewhere on the screen
                            StopHighlighting();
                            selectedUnit = null;
                            selectedSpawnUnit = Units.None;
                        }
                    }
                    
                }
            }
            else {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    void HighlightEmptyTiles() {
        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapHeight; y++) {
                if (map[x, y].isRoad && map[x, y].isEmpty && !map[x, y].isSpawn) {
                    Vector3 pos = TileCoordinatesToReal(new Vector2Int(x, y));
                    highlightingTiles.Add(Instantiate(highlightingTile, pos, Quaternion.identity));
                    highlightedPositions.Add(new Vector2Int(x,y));
                }
            }
        }
    }
    
    void HighlightTiles(List<Vector2Int> positions) {
        //destroy any previous highlighted tiles
        //StopHighlighting();
        if (betweenPhase) {
            //game is in phase between waves
            HighlightEmptyTiles();

            return;
        }
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

    public bool CheckBounds(Vector2Int position) {

        return (position.x >= 0 && position.x < mapWidth && position.y >= 0 && position.y < mapHeight);
    }

    void StopHighlighting() {
        foreach (var tile in highlightingTiles) {
            Destroy(tile);
        }

        highlightedPositions.Clear();
        highlightingTiles.Clear();

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

                map[i, j] = new MapTile();

                map[i, j].isRoad = roads.HasTile(position);

                if (spawnPositions.HasTile(position)) {

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

    public Vector3 GetNearestPositionOnGrid(Vector3 position) {
        return TileCoordinatesToReal(RealCoordinatesToNearestTile(position));
        ;
    }

    public Vector3 TileCoordinatesToReal(Vector2Int tileCoordinates) {
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

    public Vector2Int RealCoordinatesToNearestTile(Vector3 realCoordinates) {
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

    public bool SpawnEnemy(int i) {
        if (!(i >= 0 && i < allEnemies.Count)) {
            return false;
        }
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
            return false;
        }
        
        Vector3 pos = TileCoordinatesToReal(spawn);
        if (map[spawn.x, spawn.y].isEmpty && map[spawn.x, spawn.y].isRoad) {
            GameObject enemyObject = Instantiate(allEnemies[i], pos, Quaternion.identity);
            BasicEnemy enemy = enemyObject.GetComponent<BasicEnemy>();

            if (enemy != null) {
                map[spawn.x, spawn.y].isEmpty = false;
                map[spawn.x, spawn.y].hasEnemy = true;
                map[spawn.x, spawn.y].enemy = enemy;
                enemy.SetUp(map, spawn, pathsFromSpawn[spawn],this);
                enemies.Add(enemy);
            }
        }

        return true;
    }

    void SpawnUnit(Vector3 position) {
        Vector2Int tilePosition = RealCoordinatesToNearestTile(position);
        Vector3 gridPosition = GetNearestPositionOnGrid(position);

        if (!(gridPosition.x < ((-mapWidth * tileSize / 2) + roads.transform.position.x) ||
              gridPosition.x > ((mapWidth * tileSize / 2) + roads.transform.position.x) ||
              gridPosition.y < ((-mapHeight * tileSize / 2) + roads.transform.position.y) ||
              gridPosition.y > ((mapHeight * tileSize / 2) + roads.transform.position.y))) {
            if (map[tilePosition.x, tilePosition.y].isEmpty && !map[tilePosition.x, tilePosition.y].isSpawn) {
                if (selectedSpawnUnit != Units.None) {
                    
                    GameObject unitObject = Instantiate(allUnits[(int) selectedSpawnUnit-1], gridPosition,
                        Quaternion.identity);
                    BasicUnit unit = unitObject.GetComponent<BasicUnit>();
                    

                    if (unit != null) {
                        map[tilePosition.x, tilePosition.y].isEmpty = false;
                        map[tilePosition.x, tilePosition.y].hasEnemy = false;
                        map[tilePosition.x, tilePosition.y].hasUnit = true;
                        map[tilePosition.x, tilePosition.y].unit = unit;
                        unit.SetUp(map, tilePosition, this);
                        playerGold -= unit.GoldValue;
                        units.Add(unit);
                    }
                }
            }
            else {
                Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                if (!map[tilePosition.x, tilePosition.y].isEmpty) {
                    TextPopUp.Create(pos,"Cannot spawn unit on occupied tile",Color.red);
                }

                if (map[tilePosition.x, tilePosition.y].isSpawn) {
                    TextPopUp.Create(pos, "Cannot spawn unit on spawn",Color.red);
                }
            }
        }
    }

    public void EndTurnWrapper() {
        //set skip to false so animations play normally
        skip = false;
        StopHighlighting();
        selectedUnit = null;
        selectedSpawnUnit = Units.None;
        
        //show skip button
        skipButton.SetActive(true);
        StartCoroutine(EndTurn());
    }

    public void SkipTurn() {
        skip = true;
        skipButton.SetActive(false);
    }
    
    private IEnumerator EndTurn() {
        isPlayersTurn = false;
        endTurnButton.SetActive(false);
        playerGold += Properties.turnGoldIncrement;
        
        
        foreach (var unit in units) {
            unit.ResetAfterTurn();
            yield return StartCoroutine(unit.Attack());
        }
        
        if (betweenPhase) {
            betweenPhase = false;
            endOfWave = false;
            WaveText.gameObject.transform.parent.gameObject.SetActive(true);
            PhaseText.SetActive(false);
            currentWave = waves.NextWave();
            WaveText.text = "Wave " + currentWave.number;
            
        }

        yield return StartCoroutine(EnemyTurn());
    }
    
    IEnumerator EnemyTurn() {
        for (int i = enemies.Count-1 ; i >=0; --i) {
            enemies[i].ResetAfterTurn();
            yield return StartCoroutine(enemies[i].Move());
        }
        
        for (int i = enemies.Count-1; i >= 0; --i) {
            yield return StartCoroutine(enemies[i].Attack());
        }
        
        if (!endOfWave) {
            SpawnWave();
        }

        if (endOfWave && enemies.Count == 0) {
            WaveText.gameObject.transform.parent.gameObject.SetActive(false);
            PhaseText.SetActive(true);
            betweenPhase = true;
            currentDelayBetweenPartsOfWaves = 0;
        }
        isPlayersTurn = true;
        skipButton.SetActive(false);
        endTurnButton.SetActive(true);
        
        
       
    }

    private void SpawnWave() {
        if (currentDelayBetweenPartsOfWaves <= 0) {
            if (currentWave == null) {
                if (waves.Empty()) {
                    return;
                }
                currentWave = waves.NextWave();
            }

            EnemyWavePart part = currentWave.enemies[0];

            while (part.number > 0) {
                if (SpawnEnemy((int) part.id-1)) {
                    part.number--;
                }
                else {
                    break;
                }
            }

            if (part.number == 0) {
                currentWave.enemies.RemoveAt(0);
                currentDelayBetweenPartsOfWaves = Properties.delayBetweenPartsOfWaves;
            }

            if (currentWave.enemies.Count == 0) {
                currentWave = null;
                endOfWave = true;
            }
        }
        else {
            currentDelayBetweenPartsOfWaves--;
        }
    }

    //calculates positions unit can move to based on its range
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

    //calculates tile positions from range vectors
    public static List<Vector2Int> RangeVectorsToPositions(Vector2Int pos, List<Vector2Int> vectors) {
        List<Vector2Int> positions = new List<Vector2Int>();
        foreach (Vector2Int vector in vectors) {
            positions.Add(pos + vector);
        }

        return positions;
    }

    //calculates positions creature can attack from its range
    //range value is 10 times its range, cause floats
    //range is rhombus shaped
    //attack range 1: 1 up, 1 down, 1 left, 1 right
    //attack range 2: 2 up, 2 down, 2 left, 2 right, 1 diagonally
    public static List<Vector2Int> AttackRangeToRangeVectors(int range) {
       
        List<Vector2Int> vectors = new List<Vector2Int>();
        bool shorter = false;
        
        if (range % 10 == 5) {
            shorter = true;
            range /= 10;
            range++;
        }
        else {
            range /= 10;
        }
        
        for (int y = -range; y <= range; y++) {
            for (int x = -(range - Mathf.Abs(y)); x <= range - Mathf.Abs(y); x++) {
                //if the range ended with 5, the 4 furthest positions are skipped
                if (shorter && (Mathf.Abs(x) == range || Mathf.Abs(y) == range)) {
                    continue;
                }
                
                //the position of creature itself is skipped, creature cannot attack itself
                if (x == 0 && y == 0) {
                    continue;
                }

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

    private void AddAllUnits() {
        allUnits = new List<GameObject>();
        unitValues = new List<int>();

        allUnits.Add(Resources.Load<GameObject>("Prefabs/Creatures/Units/SpearManPrefab"));
        unitValues.Add(UnitProperties.SpearManGoldValue);
        
        allUnits.Add(Resources.Load<GameObject>("Prefabs/Creatures/Units/SwordsManPrefab"));
        unitValues.Add(UnitProperties.SwordsManGoldValue);
        
        allUnits.Add(Resources.Load<GameObject>("Prefabs/Creatures/Units/ArcherPrefab"));
        unitValues.Add(UnitProperties.ArcherGoldValue);
        
        allUnits.Add(Resources.Load<GameObject>("Prefabs/Creatures/Units/AxeManPrefab"));
        unitValues.Add(UnitProperties.AxeManGoldValue);
        
        allUnits.Add(Resources.Load<GameObject>("Prefabs/Creatures/Units/CrossbowManPrefab"));
        unitValues.Add(UnitProperties.CrossbowManGoldValue);
        
        allUnits.Add(Resources.Load<GameObject>("Prefabs/Creatures/Units/BerserkerPrefab"));
        unitValues.Add(UnitProperties.BerserkerGoldValue);
        
        allUnits.Add(Resources.Load<GameObject>("Prefabs/Creatures/Units/ShieldManPrefab"));
        unitValues.Add(UnitProperties.ShieldManGoldValue);
        
        allUnits.Add(Resources.Load<GameObject>("Prefabs/Creatures/Units/MagePrefab"));
        unitValues.Add(UnitProperties.MageGoldValue);
    }

    private void AddAllEnemies() {
        allEnemies = new List<GameObject>();
        
        // allEnemies.Add(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Creatures/Enemies/GoblinPrefab.prefab"));
        // allEnemies.Add(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Creatures/Enemies/OrcPrefab.prefab"));
        allEnemies.Add(Resources.Load<GameObject>("Prefabs/Creatures/Enemies/GoblinPrefab"));
        allEnemies.Add(Resources.Load<GameObject>("Prefabs/Creatures/Enemies/OrcPrefab"));
    }

    public void SelectUnitToSpawn(String unitName) {
        if (isPlayersTurn) {
            if (Enum.TryParse(unitName, true, out selectedSpawnUnit)) { }
        }
    }
    
    
    public void TestSpawnEnemy(int i) {
        if (!(i >= 0 && i < allEnemies.Count)) {
            return;
        }
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
            GameObject enemyObject = Instantiate(allEnemies[i], pos, Quaternion.identity);
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
}