using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f; // Adjust this value to change the horizontal movement speed
    [SerializeField] private float _peakHeight = 0.5f; // Adjust this value to control how high the player rises
    [SerializeField] private GameObject portalPrefab;

    private Transform _cameraTarget;
    private Portal _currentPortal;
    private LevelManager _levelMan;
    private Map _currentMap;
    private Tile _currentTile;
    private float _cellSize;
    private bool _isMoving = false;
    private bool _canMove = true;
   

    private void Start()
    {
        _cameraTarget = GameObject.FindGameObjectWithTag("CameraTarget").GetComponent<Transform>();

        ActionManager._instance.onMazeChange += OnMazeChange;
        _levelMan = LevelManager._instance;
        _cellSize = _levelMan.GetCellSize();
        _currentMap = _levelMan.GetCurrentMap();

        _currentPortal = null;
        _currentTile = null;
        transform.position = (Vector3)_levelMan.GetPlayerStartPos();
        _cameraTarget.position = transform.position;
    }

    private void OnDestroy()
    {
        ActionManager._instance.onMazeChange -= OnMazeChange;
    }

    private void Update()
    {
        if (_isMoving || !_canMove) return;

        if (Input.GetKeyDown(KeyCode.W)) { Move(Vector2.up); }
        if (Input.GetKeyDown(KeyCode.S)) { Move(Vector2.down); }
        if (Input.GetKeyDown(KeyCode.D)) { Move(Vector2.right); }
        if (Input.GetKeyDown(KeyCode.A)) { Move(Vector2.left); }
        if (Input.GetKeyDown(KeyCode.Space)) { Teleport(); }
    }

    private void Move(Vector2 vector)
    {
        if (IsMoveable(vector))
        {
            StartCoroutine(ParabolicMove(transform.position, transform.position + (Vector3)vector * _cellSize));
        }
    }

    private void Teleport()
    {
        if (_currentPortal.IsPortalOpen())
        {
            ActionManager._instance.onTeleport?.Invoke();

            transform.position = _currentPortal.GetPosition();
            _cameraTarget.position = transform.position;

            _currentPortal.SetPortalState(false);
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

        _cameraTarget.position = transform.position;

        // When Movement is Ended
        if (_currentMap.IsCrossroad(transform.position)) { OnCrossroad(); }
        if (_currentMap.IsDeadEnd(transform.position)) { OnDeadend(); }

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
                Destroy(_currentPortal.gameObject);
                _currentPortal = null;

                _canMove = false;
                ActionManager._instance.onMazeFinish?.Invoke();
            }

            _currentTile = tile;
            return true;
        }
    }

    private void OnDeadend()
    {
        if (_currentPortal != null)
        {
            _currentPortal.SetPortalState(true);
        }
    }

    private void OnCrossroad()
    {
        if (_currentPortal != null)
        {
            Destroy(_currentPortal.gameObject);
            _currentPortal = null;
        }

        GameObject currentPortalGO = Instantiate(portalPrefab, transform.position, Quaternion.identity);
        _currentPortal = currentPortalGO.GetComponent<Portal>();
    }

    private void OnMazeChange()
    {
        _currentMap = _levelMan.GetCurrentMap();
        transform.position = (Vector3)_levelMan.GetPlayerStartPos();

        _canMove = true;
    }
}
