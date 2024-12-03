using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    [Header("Map Transition")]
    [SerializeField] private RectTransform mapTransitionRect;
    [SerializeField] private float mapTransitionAnimationDuration = 0.5f;

    [Header("Teleport Effect")]
    [SerializeField] private Image teleportBG;
    [SerializeField] private float teleportStartTransitionDuration = 0.1f;
    [SerializeField] private float teleportEndTransitionDuration = 0.3f;

    void Start()
    {
        teleportBG.gameObject.SetActive(false);
        mapTransitionRect.localScale = Vector3.zero;

        ActionManager._instance.onMazeFinish += StartMapTransition;
        ActionManager._instance.onTeleport += StartTeleportTransition;
    }

    private void OnDestroy()
    {
        ActionManager._instance.onMazeFinish -= StartMapTransition;
        ActionManager._instance.onTeleport -= StartTeleportTransition;
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

    private void StartTeleportTransition()
    {
        teleportBG.gameObject.SetActive(true);
        teleportBG.DOFade(1f, teleportStartTransitionDuration).OnComplete(() =>
        {
            teleportBG.DOFade(0f, teleportEndTransitionDuration).OnComplete(() =>
            {
                teleportBG.gameObject.SetActive(false);
            });
        });

    }
}
