using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFadeController : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Coroutine fadeCor;

    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;
    
    float accumTime = 0f;

    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        StartFadeIn();
    }

    public void StartFadeIn()
    {
        if (fadeCor != null)
        {
            StopAllCoroutines();
            fadeCor = null;
        }
        fadeCor = StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.2f);
        accumTime = 0f;
        while (accumTime < fadeInTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, accumTime / fadeInTime);
            yield return null;
            accumTime += Time.deltaTime;
        }
        canvasGroup.alpha = 1f;

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3.0f);
        accumTime = 0f;
        while (accumTime < fadeOutTime)
        { 
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, accumTime / fadeOutTime);
            yield return null;
            accumTime += Time.deltaTime;
        }
        canvasGroup.alpha = 0f;
    }
}