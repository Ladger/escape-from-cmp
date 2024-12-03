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

    private List<Map> maps = new();
    private Map currentMap;
    private Vector2 playerStartPos;

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

            for (int y = 0; y < lines.Length; y++)
            {
                List<Tile> row = new List<Tile>();
                string line = lines[y].TrimEnd();
                columnCount = line.Length;

                for (int x = 0; x < line.Length; x++)
                {
                    // Map characters to ElementType
                    TileType type = TileType.Blockage;
                    if (line[x] == ' ') type = TileType.Path;
                    if (line[x] == 'F') type = TileType.Finish;
                    if (line[x] == 'P') type = TileType.Player;

                    Vector2 position = new Vector2(x, -y);
                    Tile tile = new Tile(position, type);
                    row.Add(tile);
                }

                tiles.Add(row);
            }

            Map map = new Map(tiles, rowCount, columnCount);
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

                switch (tile.tileType)
                {
                    case TileType.Blockage:
                        Instantiate(wallPrefab, worldPosition, Quaternion.identity, parent);
                        break;

                    case TileType.Path:
                        if (x % 2 == checker)
                        {
                            Instantiate(pathPrefab, worldPosition, Quaternion.identity, parent);
                        }
                        else
                        {
                            Instantiate(pathPrefab2, worldPosition, Quaternion.identity, parent);
                        }
                        
                        break;

                    case TileType.Finish:
                        Instantiate(finishPrefab, worldPosition, Quaternion.identity, parent);
                        break;

                    case TileType.Player:
                        playerStartPos = worldPosition;
                        Instantiate(pathPrefab, worldPosition, Quaternion.identity, parent);
                        break;
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
    public Vector2 GetPlayerStartPos() { return playerStartPos; }
    public float GetCellSize() { return cellSize; }
}

public class Map
{
    public List<List<Tile>> tiles;
    public int rowCount;
    public int columnCount;

    public Map(List<List<Tile>> tiles, int rowCount, int columnCount)
    {
        this.tiles = tiles;
        this.rowCount = rowCount;
        this.columnCount = columnCount;
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
        if (GetTile(position).tileType == TileType.Finish) { return false; }

        int roadCount = 0;
        float cellSize = LevelManager._instance.GetCellSize();

        if (GetTile(position + Vector2.up * cellSize).tileType == TileType.Path) { roadCount++; }
        if (GetTile(position + Vector2.down * cellSize).tileType == TileType.Path) { roadCount++; }
        if (GetTile(position + Vector2.right * cellSize).tileType == TileType.Path) { roadCount++; }
        if (GetTile(position + Vector2.left * cellSize).tileType == TileType.Path) { roadCount++; }


        if (roadCount > 2) { return true; }
        else { return false; }
    }

    public bool IsDeadEnd(Vector2 position)
    {
        if (GetTile(position).tileType == TileType.Finish) { return false; }

        int blockageCount = 0;
        float cellSize = LevelManager._instance.GetCellSize();

        if (GetTile(position + Vector2.up * cellSize).tileType == TileType.Blockage) { blockageCount++; }
        if (GetTile(position + Vector2.down * cellSize).tileType == TileType.Blockage) { blockageCount++; }
        if (GetTile(position + Vector2.right * cellSize).tileType == TileType.Blockage) { blockageCount++; }
        if (GetTile(position + Vector2.left * cellSize).tileType == TileType.Blockage) { blockageCount++; }

        if (blockageCount > 2) { return true; }
        else return false;
    }
}

public class Tile
{
    public Vector2 position;
    public TileType tileType;

    public Tile(Vector2 position, TileType elementType)
    {
        this.position = position;
        this.tileType = elementType;
    }
}

public enum TileType
{
    Blockage,
    Path,
    Finish,
    Player
}