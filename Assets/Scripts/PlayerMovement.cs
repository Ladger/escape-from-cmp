using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Map currentMap;
    private float cellSize;

    private void Start()
    {
        LevelManager levelMan = LevelManager._instance;
        cellSize = levelMan.GetCellSize();
        currentMap = levelMan.GetCurrentMap();

        transform.position = (Vector3)levelMan.GetPlayerStartPos();
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
        if (IsMoveable(vector)) { transform.position += (Vector3)vector * cellSize; }
    }

    private bool IsMoveable(Vector2 vector)
    {
        Tile tile = currentMap.GetTile(transform.position + (Vector3)vector);
        
        if (tile.tileType == TileType.Blockage)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
