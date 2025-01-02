using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Animator yhsAnim;

    [Header("Background")]
    [SerializeField] private SpriteRenderer bgImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        FadeInBG();
        yield return new WaitForSeconds(fadeDuration);

        PlayTyping();

        for (int i = 0; i < 6; i++)
        {
            AnimOn();
            yield return new WaitForSeconds(0.5f);
            AnimOff();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void FadeInBG()
    {
        bgImage.DOFade(0f, fadeDuration);
    }

    private void PlayTyping()
    {
        AudioManager._instance.PlaySFX("typing");
    }

    private void AnimOn()
    {
        yhsAnim.SetBool("isTyping", true);
    }

    private void AnimOff()
    {
        yhsAnim.SetBool("isTyping", false);
    }
}
