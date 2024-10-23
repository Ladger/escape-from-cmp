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
    private Vector2 playerStartPos;

    protected override void Awake()
    {
        base.Awake();

        LoadMaps("Assets/Resources/maps.txt");
        BuildMap(0);

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
                    ElementType type = ElementType.Blockage;
                    if (line[x] == ' ') type = ElementType.Path;
                    if (line[x] == 'F') type = ElementType.Finish;
                    if (line[x] == 'P') type = ElementType.Player;

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

    private void BuildMap(int mapIndex)
    {
        Map map = maps[mapIndex];

        for (int y = 0; y < map.rowCount; y++)
        {
            int checker = y % 2;
            for (int x = 0; x < map.columnCount; x++)
            {
                

                Tile tile = map.tiles[y][x];
                Vector3 worldPosition = new Vector3(tile.position.x * cellSize, tile.position.y * cellSize, 0);

                switch (tile.elementType)
                {
                    case ElementType.Blockage:
                        Instantiate(wallPrefab, worldPosition, Quaternion.identity);
                        break;

                    case ElementType.Path:
                        if (x % 2 == checker)
                        {
                            Instantiate(pathPrefab, worldPosition, Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(pathPrefab2, worldPosition, Quaternion.identity);
                        }
                        
                        break;

                    case ElementType.Finish:
                        Instantiate(finishPrefab, worldPosition, Quaternion.identity);
                        break;

                    case ElementType.Player:
                        playerStartPos = worldPosition;
                        Instantiate(pathPrefab, worldPosition, Quaternion.identity);
                        Instantiate(playerPrefab, worldPosition, Quaternion.identity);
                        break;
                }
            }
        }
    }


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
}

public class Tile
{
    public Vector2 position;
    public ElementType elementType;

    public Tile(Vector2 position, ElementType elementType)
    {
        this.position = position;
        this.elementType = elementType;
    }
}

public enum ElementType
{
    Blockage,
    Path,
    Finish,
    Player
}