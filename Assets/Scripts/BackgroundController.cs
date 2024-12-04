using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    [Header("Map Transition")]
    [SerializeField] private Image mapTransition;
    [SerializeField] private float mapStartTransitionDuration = 0.1f;
    [SerializeField] private float mapEndTransitionDuration = 0.3f;

    [Header("Teleport Effect")]
    [SerializeField] private Image teleportBG;
    [SerializeField] private float teleportStartTransitionDuration = 0.1f;
    [SerializeField] private float teleportEndTransitionDuration = 0.3f;

    void Start()
    {
        teleportBG.gameObject.SetActive(false);

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


        mapTransition.gameObject.SetActive(true);
        mapTransition.DOFade(1f, mapStartTransitionDuration).OnComplete(() =>
        {
            GameManager._instance.ChangeMap();
            mapTransition.DOFade(0f, mapEndTransitionDuration).OnComplete(() =>
            {
                mapTransition.gameObject.SetActive(false);
            });
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
