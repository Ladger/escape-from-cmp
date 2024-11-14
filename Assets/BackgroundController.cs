using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.5f;

    private RectTransform _rect;

    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _rect.localScale = Vector3.zero;

        ActionManager._instance.onMazeFinish += StartTransition;   
    }

    private void OnDestroy()
    {
        ActionManager._instance.onMazeFinish -= StartTransition;
    }

    private void StartTransition()
    {
        _rect.DOScale(Vector3.one, animationDuration)
            .OnComplete(() => 
            {
                GameManager._instance.ChangeMap();
                _rect.DOScale(Vector3.zero, animationDuration);
            });
    }
}
