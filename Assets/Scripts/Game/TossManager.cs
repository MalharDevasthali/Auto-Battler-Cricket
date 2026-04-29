using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TossManager : MonoBehaviour
{
    [SerializeField] private Button tossButton;
    [SerializeField] private Button chooseBattingButton;
    [SerializeField] private Button chooseBowlingButton;
    [SerializeField] private TextMeshProUGUI tossFeedbackText;

    [SerializeField] private RectTransform coinObject;
    [SerializeField] private Sprite HeadsCoinSprite;
    [SerializeField] private Sprite TailsCoinSprite;
    [SerializeField] private CanvasGroup TeamGeneratorCanvasGroup;
    [SerializeField] private CanvasGroup tossCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;
    
    private const string CoinTossAnimationStateName = "CoinTossAnimation";

    private Animator coinAnimator;
    private Image coinImage;
    private Coroutine tossCoroutine;
    private bool isTossWon;


    private void Awake()
    {
        if (tossButton != null)
            tossButton.onClick.AddListener(OnTossButtonClick);

        if (chooseBattingButton != null)
            chooseBattingButton.onClick.AddListener(OnChooseBattingButtonClick);

        if (chooseBowlingButton != null)
            chooseBowlingButton.onClick.AddListener(OnChooseBowlingButtonClick);

        if (coinObject != null)
        {
            coinAnimator = coinObject.GetComponent<Animator>();
            coinImage = coinObject.GetComponent<Image>();
        }

        SetCanvasGroupState(TeamGeneratorCanvasGroup, 1f);
        SetCanvasGroupState(tossCanvasGroup, 0f);
        SetChoiceButtonsActive(false);
        SetFeedbackText(string.Empty);
    }

    private void OnTossButtonClick()
    {
        if (coinImage == null)
        {
            Debug.LogError("Toss button clicked, but coinObject has no Image component.");
            return;
        }

        if (tossCoroutine != null)
            StopCoroutine(tossCoroutine);

        tossCoroutine = StartCoroutine(TossCoin());
        SetFeedbackText(string.Empty);
    }

    private IEnumerator TossCoin()
    {
       
        if (tossButton != null)
            tossButton.interactable = false;

        SetChoiceButtonsActive(false);

        yield return FadeCanvasGroup(TeamGeneratorCanvasGroup, 0f);
        yield return FadeCanvasGroup(tossCanvasGroup, 1f);
        
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
        isTossWon = Random.Range(0, 2) == 0;


        if (resultSprite != null)
            SetCoinSprite(resultSprite);

        Debug.Log($"Coin Toss Result: {resultText}");

        if (isTossWon)
        {
            SetFeedbackText($"Toss Result: {resultText}\nYou won the toss. Choose batting or bowling.");
            SetChoiceButtonsActive(true);
        }
        else
        {
            bool computerChoosesBatting = Random.Range(0, 2) == 0;
            Innings playerInnings = computerChoosesBatting ? Innings.Bowling : Innings.Batting;
            string computerChoiceText = computerChoosesBatting ? "batting" : "bowling";

            SetCurrentInnings(playerInnings);
            SetFeedbackText($"Toss Result: {resultText}\nYou lost the toss. Computer chose {computerChoiceText}.");

            yield return new WaitForSeconds(2f);
            yield return FadeTossCanvasOutAndTeamGeneratorIn();
            isTossWon = false;
        }

        tossCoroutine = null;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha)
    {
        if (canvasGroup == null)
            yield break;

        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        canvasGroup.interactable = targetAlpha > 0f;
        canvasGroup.blocksRaycasts = targetAlpha > 0f;

        if (fadeDuration <= 0f)
        {
            canvasGroup.alpha = targetAlpha;
            yield break;
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }
    
        canvasGroup.alpha = targetAlpha;
    }

    private void SetCoinSprite(Sprite sprite)
    {
        if (coinImage != null)
            coinImage.sprite = sprite;
    }

    private void OnChooseBattingButtonClick()
    {
        HandlePlayerChoice(Innings.Batting, "batting");
    }

    private void OnChooseBowlingButtonClick()
    {
        HandlePlayerChoice(Innings.Bowling, "bowling");
    }

    private void HandlePlayerChoice(Innings selectedInnings, string selectedChoiceText)
    {
        if (!isTossWon)
            return;

        SetCurrentInnings(selectedInnings);
        SetChoiceButtonsActive(false);
        SetFeedbackText($"You won the toss and chose {selectedChoiceText}.");
        isTossWon = false;
        StartCoroutine(CompleteTossFlowAfterDelay());
    }

    private IEnumerator CompleteTossFlowAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        yield return FadeTossCanvasOutAndTeamGeneratorIn();

        if (tossButton != null)
            tossButton.interactable = true;
    }

    private IEnumerator FadeTossCanvasOutAndTeamGeneratorIn()
    {
        yield return FadeCanvasGroup(tossCanvasGroup, 0f);
        yield return FadeCanvasGroup(TeamGeneratorCanvasGroup, 1f);
    }

    private void SetCurrentInnings(Innings innings)
    {
        if (ServiceLocator.Instance == null || ServiceLocator.Instance.GameService == null)
        {
            Debug.LogError("Unable to set innings. GameService is missing from ServiceLocator.");
            return;
        }

        ServiceLocator.Instance.GameService.SetCurrentInnings(innings);
        Debug.Log($"Current Innings: {innings}");
    }

    private void SetChoiceButtonsActive(bool isActive)
    {
        if (chooseBattingButton != null)
            chooseBattingButton.gameObject.SetActive(isActive);

        if (chooseBowlingButton != null)
            chooseBowlingButton.gameObject.SetActive(isActive);
    }

    private void SetFeedbackText(string feedback)
    {
        if (tossFeedbackText != null)
            tossFeedbackText.text = feedback;

        if (!string.IsNullOrEmpty(feedback))
            Debug.Log(feedback);
    }

    private void SetCanvasGroupState(CanvasGroup canvasGroup, float alpha)
    {
        if (canvasGroup == null)
            return;

        canvasGroup.alpha = alpha;
        canvasGroup.interactable = alpha > 0f;
        canvasGroup.blocksRaycasts = alpha > 0f;
    }
}
