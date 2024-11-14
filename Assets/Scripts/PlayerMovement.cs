
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private LevelManager _levelMan;
    private Map _currentMap;
    private Tile _currentTile;
    private float _cellSize;

    private void Start()
    {
        ActionManager._instance.onMazeChange += OnMazeChange;
        _levelMan = LevelManager._instance;
        _cellSize = _levelMan.GetCellSize();
        _currentMap = _levelMan.GetCurrentMap();

        _currentTile = null;
        transform.position = (Vector3)_levelMan.GetPlayerStartPos();
    }

    private void OnDestroy()
    {
        ActionManager._instance.onMazeChange -= OnMazeChange;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) { Move(Vector2.up); }
        else if (Input.GetKeyDown(KeyCode.S)) { Move(Vector2.down); }
        else if (Input.GetKeyDown(KeyCode.D)) { Move(Vector2.right); }
        else if (Input.GetKeyDown(KeyCode.A)) { Move(Vector2.left); }
    }

    private void Move(Vector2 vector)
    {
     
        if (IsMoveable(vector)) { 
            transform.position += (Vector3)vector * _cellSize;
        }
    }

    private bool IsMoveable(Vector2 vector)
    {
        Tile tile = _currentMap.GetTile(transform.position + (Vector3)vector);
        
        if (tile.tileType == TileType.Blockage)
        {
            return false;
        }
        else
        {
            if (tile.tileType == TileType.Finish)
            {
                ActionManager._instance.onMazeFinish?.Invoke();
            }
            
            _currentTile = tile;
            return true;
        }
    }

    private void OnMazeChange()
    {
        _currentMap = _levelMan.GetCurrentMap();
        transform.position = (Vector3)_levelMan.GetPlayerStartPos();
    }
}
