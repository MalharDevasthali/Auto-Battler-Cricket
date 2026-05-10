using System.Collections;
using UnityEngine;

public class UICanvasManager : MonoBehaviour
{
    [SerializeField] private float canvasFadeDuration;

    [SerializeField] private CanvasGroup battleSceneCanvasGroup;
    [SerializeField] private CanvasGroup matchFinishedCanvasGroup;

    private Coroutine fadeCoroutine; 


    public IEnumerator FadeOutFadeInBattleSceneCanvas()
    {
        yield return FadeCanvasGroup(battleSceneCanvasGroup, 0f);
        yield return FadeCanvasGroup(battleSceneCanvasGroup, 1f);
    }


    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha)
    {
        if (canvasGroup == null)
            yield break;

        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        canvasGroup.interactable = targetAlpha > 0f;
        canvasGroup.blocksRaycasts = targetAlpha > 0f;

        if (canvasFadeDuration <= 0f)
        {
            canvasGroup.alpha = targetAlpha;
            yield break;
        }

        while (elapsedTime < canvasFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / canvasFadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }


}
