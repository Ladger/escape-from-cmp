using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f; // Adjust this value to change the horizontal movement speed
    [SerializeField] private float _peakHeight = 0.5f; // Adjust this value to control how high the player rises

    private LevelManager _levelMan;
    private Map _currentMap;
    private Tile _currentTile;
    private float _cellSize;
    private bool _isMoving = false;
   

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

    private void Update()
    {
        if (_isMoving) return; // Prevent new input while moving

        if (Input.GetKeyDown(KeyCode.W)) { Move(Vector2.up); }
        else if (Input.GetKeyDown(KeyCode.S)) { Move(Vector2.down); }
        else if (Input.GetKeyDown(KeyCode.D)) { Move(Vector2.right); }
        else if (Input.GetKeyDown(KeyCode.A)) { Move(Vector2.left); }
    }

    private void Move(Vector2 vector)
    {
        if (IsMoveable(vector))
        {
            StartCoroutine(ParabolicMove(transform.position, transform.position + (Vector3)vector * _cellSize));
        }
    }

    private IEnumerator ParabolicMove(Vector3 start, Vector3 end)
    {
        _isMoving = true;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            float t = elapsedTime;
            Vector3 horizontalPosition = Vector3.Lerp(start, end, t);
            float verticalOffset = _peakHeight * Mathf.Sin(Mathf.PI * t);
            transform.position = new Vector3(horizontalPosition.x, horizontalPosition.y + verticalOffset, horizontalPosition.z);

            elapsedTime += Time.deltaTime * _moveSpeed;
            yield return null;
        }

        transform.position = end;
        _isMoving = false;
    }

    private bool IsMoveable(Vector2 vector)
    {
        Tile tile = _currentMap.GetTile(transform.position + (Vector3)vector * _cellSize);

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
