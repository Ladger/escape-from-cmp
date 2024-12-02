using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [Header("Map Transition")]
    [SerializeField] private RectTransform mapTransitionRect;
    [SerializeField] private float mapTransitionAnimationDuration = 0.5f;

    void Start()
    {
        mapTransitionRect.localScale = Vector3.zero;

        ActionManager._instance.onMazeFinish += StartMapTransition;   
    }

    private void OnDestroy()
    {
        ActionManager._instance.onMazeFinish -= StartMapTransition;
    }

    private void StartMapTransition()
    {
        mapTransitionRect.DOScale(Vector3.one, mapTransitionAnimationDuration)
            .OnComplete(() => 
            {
                GameManager._instance.ChangeMap();
                mapTransitionRect.DOScale(Vector3.zero, mapTransitionAnimationDuration);
            });
    }
}
