using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private GameManager _gameManager;
    private TextMeshProUGUI _timerText;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager._instance;
       _timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        UpdateTimer(_gameManager.GetTimer());
    }

    private void UpdateTimer(float timer)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);

        string timeText = "";
        if (timer >= 60f)
        {
            timeText = timeSpan.ToString("mm':'ss':'ff");
        }
        else
        {
            timeText = timeSpan.ToString("ss':'ff");
        }
        

        _timerText.text = timeText;
    }
}
