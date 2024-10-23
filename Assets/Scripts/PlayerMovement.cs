using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float cellSize;
    private Vector2 playerStartPos;
    private Vector2 playerGridPos;

    private void Start()
    {
        playerStartPos = LevelManager._instance.GetPlayerStartPos();
        cellSize = LevelManager._instance.GetCellSize();

        transform.position = (Vector3)playerStartPos;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) { Move(Vector2.up); }
        else if (Input.GetKeyDown(KeyCode.S)) { Move(Vector2.down); }
        else if (Input.GetKeyDown(KeyCode.D)) { Move(Vector2.right); }
        else if (Input.GetKeyDown(KeyCode.A)) { Move(Vector2.left); }
    }

    void Move(Vector2 vector)
    {
        transform.position += (Vector3)vector * cellSize;
    }
}
