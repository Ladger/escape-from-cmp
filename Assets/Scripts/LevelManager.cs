using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private float cellSize = 1;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject pathPrefab;
    [SerializeField] private GameObject pathPrefab2;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject finishPrefab;
    [SerializeField] private GameObject blinkPrefab;

    private List<Map> maps = new();
    private Map currentMap;

    [Button]
    public void PutMap()
    {
        LoadMaps("Assets/Resources/maps.txt");
        BuildMap(0);
    }

    protected override void Awake()
    {
        base.Awake();

        LoadMaps("Assets/Resources/maps.txt");
    }

    private void LoadMaps(string filePath)
    {
        string[] rawMazes = File.ReadAllText(filePath).Split(new string[] { "---" }, System.StringSplitOptions.RemoveEmptyEntries);

        maps.Clear();

        int rowCount = 0;
        int columnCount = 0;
        for (int mapIndex = 0; mapIndex < rawMazes.Length; mapIndex++)
        {
            List<List<Tile>> tiles = new List<List<Tile>>();
            string[] lines = rawMazes[mapIndex].Trim().Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            rowCount = lines.Length;

            Vector2 playerPos = Vector2.zero;
            for (int y = 0; y < lines.Length; y++)
            {
                List<Tile> row = new List<Tile>();
                string line = lines[y].TrimEnd();
                columnCount = line.Length;

                for (int x = 0; x < line.Length; x++)
                {
                    bool blink = false;

                    // Map characters to ElementType
                    TileType type = TileType.Blockage;

                    if (line[x] == ' ') {
                        type = TileType.Path;
                        blink = true; 
                    
                    }
                    if (line[x] == 'F') type = TileType.Finish;

                    if (line[x] == 'P')
                    {
                        type = TileType.Path;
                        playerPos = new Vector2(x, -y);
                    }

                    Vector2 position = new Vector2(x, -y);
                    Tile tile = new Tile(position, type)
                    {
                        hasBlink = blink
                    };
                    row.Add(tile);
                }

                tiles.Add(row);
            }

            Map map = new Map(tiles, rowCount, columnCount)
            {
                playerStartPos = playerPos,
            };
            maps.Add(map);
        }
    }

    public void BuildMap(int mapIndex, Transform parent = null)
    {
        if (maps.Count == 0)
        {
            Debug.LogError("No map exists to build.");
            return;
        }

        if (maps.Count == mapIndex)
        {
            Debug.LogError("There is no more maps to build.");
            return;
        }


        parent = gameObject.transform;
        Map map = maps[mapIndex];
        currentMap = map;

        for (int y = 0; y < map.rowCount; y++)
        {
            int checker = y % 2;
            for (int x = 0; x < map.columnCount; x++)
            {
                

                Tile tile = map.tiles[y][x];
                Vector3 worldPosition = new Vector3(tile.position.x * cellSize, tile.position.y * cellSize, 0);

                GameObject tileParent = parent.gameObject;
                switch (tile.tileType)
                {
                    case TileType.Blockage:
                        tileParent = Instantiate(wallPrefab, worldPosition, Quaternion.identity, parent);
                        break;

                    case TileType.Path:
                        if (x % 2 == checker)
                        {
                            tileParent = Instantiate(pathPrefab, worldPosition, Quaternion.identity, parent);
                        }
                        else
                        {
                            tileParent = Instantiate(pathPrefab2, worldPosition, Quaternion.identity, parent);
                        }
                        
                        break;

                    case TileType.Finish:
                        tileParent = Instantiate(finishPrefab, worldPosition, Quaternion.identity, parent);
                        break;
                }

                if (tile.hasBlink)
                {
                    tile.blink = Instantiate(blinkPrefab, worldPosition, blinkPrefab.transform.rotation, tileParent.transform);
                }
            }
        }
    }

    public void ClearMap()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        currentMap = null;
    }

    public Map GetCurrentMap() { return currentMap; }
    public float GetCellSize() { return cellSize; }
    public int GetMapCount() { return maps.Count; }
}

public class Map
{
    public List<List<Tile>> tiles;
    public int rowCount;
    public int columnCount;
    public Vector2 playerStartPos;

    public Map(List<List<Tile>> tiles, int rowCount, int columnCount)
    {
        this.tiles = tiles;
        this.rowCount = rowCount;
        this.columnCount = columnCount;
        this.playerStartPos = Vector2.zero;
    }

    public Tile GetTile(Vector2 tilePosition)
    {
        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                if (tiles[row][col].position == tilePosition)
                {
                    return tiles[row][col];
                }
            }
        }

        Debug.LogError("Tile has not found");
        return null;
    }

    public bool IsCrossroad(Vector2 position)
    {
        return IsDeadOrCross(position, TileType.Path);
    }

    public bool IsDeadEnd(Vector2 position)
    {
        return IsDeadOrCross(position, TileType.Blockage);
    }

    private bool IsDeadOrCross(Vector2 position, TileType tileType)
    {
        int counter = 0;

        int column = (int)position.x;
        int row = -(int)position.y;

        // Check up
        if (row > 0 && tiles[row - 1][column].tileType == tileType) {
            counter++;
        }

        // Check down
        if (row < tiles.Count - 1 && tiles[row + 1][column].tileType == tileType) {
            counter++; 
        }

        // Check left
        if (column > 0 && tiles[row][column - 1].tileType == tileType){
            counter++;
        }

        // Check right
        if (column < tiles[row].Count - 1 && tiles[row][column + 1].tileType == tileType) {
            counter++;
        }

        return counter > 2;
    }
}

public class Tile
{
    public Vector2 position;
    public TileType tileType;
    public GameObject blink;
    public bool hasBlink;

    public Tile(Vector2 position, TileType elementType)
    {
        this.position = position;
        this.tileType = elementType;
        this.blink = null;
        this.hasBlink = true;
    }
}

public enum TileType
{
    Blockage,
    Path,
    Finish
}