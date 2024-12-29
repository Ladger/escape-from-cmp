using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour
{
    private TextMeshProUGUI _scoreboardText;
    private int _currentScore;

    private void Start()
    {
        ActionManager._instance.onBlinkCollect += ChangeScore;

        _scoreboardText = GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        ActionManager._instance.onBlinkCollect += ChangeScore;
    }

    private void ChangeScore(Vector2 pos, int point)
    {
        _currentScore += point;
        _scoreboardText.text = _currentScore.ToString();
    }
}
