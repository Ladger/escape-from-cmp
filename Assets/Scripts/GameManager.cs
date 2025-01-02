using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int blinkScorePoint = 50;
    [SerializeField] private float timer = 120f;
    private int mapCounter = 0;

    private float originalTimer;
    private bool isGameOver = false;

    private int _currentScore;

    private void Start()
    {
        originalTimer = timer;
        BuildMap(mapCounter);
    }

    private void Update()
    {
        if (isGameOver) return;
        if (timer <= 0)
        {
            isGameOver = true;
            ActionManager._instance.onGameEnd?.Invoke(EndType.Lose);
        }

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

        if (mapCounter == LevelManager._instance.GetMapCount())
        {
            isGameOver = true;
            ActionManager._instance.onGameEnd?.Invoke(EndType.Win);
            return;
        }

        LevelManager._instance.BuildMap(mapCounter);
        ActionManager._instance.onMazeChange?.Invoke();
    }

    public void UpdateScore(Vector2 pos)
    {
        _currentScore += blinkScorePoint;
        ActionManager._instance.onBlinkCollect?.Invoke(pos, blinkScorePoint);
    }

    public void ResetGame()
    {
        timer = originalTimer;
        isGameOver = false;

        mapCounter = 0;

        _currentScore = 0;
    }

    public int GetMapCounter() { return mapCounter; }
    public float GetTimer() { return timer; }
    public int GetScore() { return _currentScore; }
}

public enum EndType
{
    Win,
    Lose
}
