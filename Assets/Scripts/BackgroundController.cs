using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private float typingDuration = 0.03f;

    [Header("Map Transition")]
    [SerializeField] private Image mapTransition;
    [SerializeField] private float mapStartTransitionDuration = 0.1f;
    [SerializeField] private float mapEndTransitionDuration = 0.3f;

    [Header("Teleport Effect")]
    [SerializeField] private Image teleportBG;
    [SerializeField] private float teleportStartTransitionDuration = 0.1f;
    [SerializeField] private float teleportEndTransitionDuration = 0.3f;

    [Header("Win Screen")]
    [SerializeField] private Image winBG;
    [SerializeField] private TextMeshProUGUI winTextMesh;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject winStuff;
    [SerializeField] private float winStartTransitionDuration = 0.1f;
    [SerializeField][TextArea] private string winText;

    [Header("Lose Screen")]
    [SerializeField] private Image loseBG;
    [SerializeField] private TextMeshProUGUI loseTextMesh;
    [SerializeField] private GameObject loseStuff;
    [SerializeField] private float loseStartTransitionDuration = 0.1f;
    [SerializeField][TextArea] private string loseText;

    private EndType _endType;
    private GameObject _stuff;

    void Start()
    {
        teleportBG.gameObject.SetActive(false);

        ActionManager._instance.onMazeFinish += StartMapTransition;
        ActionManager._instance.onTeleport += StartTeleportTransition;
        ActionManager._instance.onGameEnd += StartEndGameBackground;
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

    private void StartEndGameBackground(EndType endType)
    {
        AudioManager._instance.StopMusic();
        if (endType == EndType.Win) AudioManager._instance.PlaySFX("win");
        else AudioManager._instance.PlaySFX("fail");

        _endType = endType;

        TextMeshProUGUI mesh = null;
        Image image = null;
        _stuff = null;
        string endText = "";

        switch (endType)
        {
            case EndType.Win:
                mesh = winTextMesh;
                image = winBG;
                _stuff = winStuff;
                endText = winText;
                break;
            case EndType.Lose:
                mesh = loseTextMesh;
                image = loseBG;
                _stuff = loseStuff;
                endText = loseText;
                break;
        }

        image.gameObject.SetActive(true);
        teleportBG.DOFade(1f, teleportStartTransitionDuration).OnComplete(() =>
        {
            StartCoroutine(TypeSentence(endText, mesh, OnTypeSentenceComplete));
        });
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI mesh, System.Action onComplete)
    {
        string currentSentence = "";
        foreach (char letter in sentence.ToCharArray())
        {
            currentSentence += letter;
            AudioManager._instance.PlaySFX("move_idle");
            mesh.text = currentSentence;
            yield return new WaitForSeconds(typingDuration);
        }

        onComplete?.Invoke();
    }

    private void OnTypeSentenceComplete()
    {
        if (_endType == EndType.Win)
        {
            scoreText.text = "Score: " + GameManager._instance.GetScore();
            scoreText.gameObject.SetActive(true);
        }
        _stuff.SetActive(true);
    }
}
