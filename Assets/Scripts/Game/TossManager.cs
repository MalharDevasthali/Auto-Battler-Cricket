using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TossManager : MonoBehaviour
{
    [SerializeField] private Button tossButton;
    [SerializeField] private GameObject coinObject;
    [SerializeField] private Sprite HeadsCoinSprite;
    [SerializeField] private Sprite TailsCoinSprite;
    [SerializeField] private CanvasGroup TeamGeneratorCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;
    
    private const string CoinTossAnimationStateName = "CoinTossAnimation";

    private Animator coinAnimator;
    private SpriteRenderer coinSpriteRenderer;
    private Coroutine tossCoroutine;


    private void Awake()
    {
        if (tossButton != null)
            tossButton.onClick.AddListener(OnTossButtonClick);

        if (coinObject != null)
        {
            coinAnimator = coinObject.GetComponent<Animator>();
            coinSpriteRenderer = coinObject.GetComponent<SpriteRenderer>();
        }
    }

    private void OnTossButtonClick()
    {
        if (coinSpriteRenderer == null)
            return;

        if (tossCoroutine != null)
            StopCoroutine(tossCoroutine);

        tossCoroutine = StartCoroutine(TossCoin());
    }

    private IEnumerator TossCoin()
    {
        if (tossButton != null)
            tossButton.interactable = false;

        yield return FadeCanvasGroup(0f);
        
        float animationLength = 0f;

        if (coinAnimator != null)
        {
            coinAnimator.enabled = true;
            coinAnimator.Play(CoinTossAnimationStateName, 0, 0f);
            coinAnimator.Update(0f);
            animationLength = coinAnimator.GetCurrentAnimatorStateInfo(0).length;
        }

        if (animationLength > 0f)
            yield return new WaitForSeconds(animationLength);

        if (coinAnimator != null)
            coinAnimator.enabled = false;

        bool isHeads = Random.Range(0, 2) == 0;
        Sprite resultSprite = isHeads ? HeadsCoinSprite : TailsCoinSprite;
        string resultText = isHeads ? "Heads" : "Tails";


        if (resultSprite != null)
            coinSpriteRenderer.sprite = resultSprite;

        Debug.Log($"Coin Toss Result: {resultText}");

        yield return new WaitForSeconds(1f);
        yield return FadeCanvasGroup(1f);

        if (tossButton != null)
            tossButton.interactable = true;

        tossCoroutine = null;
    }

    private IEnumerator FadeCanvasGroup(float targetAlpha)
    {
        if (TeamGeneratorCanvasGroup == null)
            yield break;

        float startAlpha = TeamGeneratorCanvasGroup.alpha;
        float elapsedTime = 0f;

        TeamGeneratorCanvasGroup.interactable = targetAlpha > 0f;
        TeamGeneratorCanvasGroup.blocksRaycasts = targetAlpha > 0f;

        if (fadeDuration <= 0f)
        {
            TeamGeneratorCanvasGroup.alpha = targetAlpha;
            yield break;
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            TeamGeneratorCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }
    
        TeamGeneratorCanvasGroup.alpha = targetAlpha;
    }
}
