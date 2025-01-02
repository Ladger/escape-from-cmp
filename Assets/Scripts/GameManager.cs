using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float timer = 120f;
    private int mapCounter = 0;

    private void Start()
    {
        BuildMap(mapCounter);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    public void BuildMap(int mapIndex = 0)
    {
        LevelManager._instance.BuildMap(mapCounter);
    }

    public void ChangeMap()
    {
        LevelManager._instance.ClearMap();

        mapCounter++;
        LevelManager._instance.BuildMap(mapCounter);
        ActionManager._instance.onMazeChange?.Invoke();
    }

    public int GetMapCounter() { return mapCounter; }
    public float GetTimer() { return timer; }
}
