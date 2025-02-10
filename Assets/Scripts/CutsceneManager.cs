using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject yhs;
    [SerializeField] private Animator yhsAnim;

    [SerializeField] private Light2D computerLight;
    [SerializeField] private Color redColor;

    [Header("Background")]
    [SerializeField] private SpriteRenderer bgImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {


        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        AudioManager._instance.PlaySFX("yhs");
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

        PlayTyping();

        for (int i = 0; i < 6; i++)
        {
            AnimOn();
            yield return new WaitForSeconds(0.5f);
            AnimOff();
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);

        computerLight.color = redColor;
        AudioManager._instance.PlaySFX("portal_open");


        yield return new WaitForSeconds(4f);

        bgImage.color = new Color(0f,0f,0f,255f);
        AudioManager._instance.PlaySFX("teleport");

        yhs.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
        bgImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("SampleScene");
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

    public void Skip()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
