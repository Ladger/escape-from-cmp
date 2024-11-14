using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int mapCounter = 0;

    private void Start()
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
}
