using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI ballText;
    [SerializeField] private Button startMatchButton;
    [SerializeField] private Button playButton;

    [Header("Bowler UI")]
    [SerializeField] private Image bowlerImage;
    [SerializeField] private TextMeshProUGUI bowlerNameText;
    [SerializeField] private TextMeshProUGUI bowlerAbilityText;
    [SerializeField] private TextMeshProUGUI bowlerBowlingPowerText;

    [Header("Batsman UI")]
    [SerializeField] private Image batsmanImage;
    [SerializeField] private TextMeshProUGUI batsmanNameText;
    [SerializeField] private TextMeshProUGUI batsmanAbilityText;
    [SerializeField] private TextMeshProUGUI battingPowerText;
    [SerializeField] private TextMeshProUGUI defenceText;

    [Header("UI Text Effects")]
    [SerializeField] private TextMeshProUGUI defenceTextEffects;
    [SerializeField] private TextMeshProUGUI battingPowerTextEffects;

    public void SetStartMatchInteractable(bool isEnabled)
    {
        if (startMatchButton != null)
            startMatchButton.enabled = isEnabled;
    }

    public void SetPlayInteractable(bool isEnabled)
    {
        if (playButton != null)
            playButton.enabled = isEnabled;
    }

    public void UpdateScore(int totalRuns, int wickets)
    {
        if (scoreText != null)
            scoreText.SetText($"Score: {totalRuns}/{wickets}");
    }

    public void UpdateBallText(int ball)
    {
        if (ballText != null)
            ballText.SetText($"Ball {ball}/6");
    }

    public void LoadBowler(PlayerData bowler)
    {
        if (bowler == null) return;

        bowlerImage.sprite = bowler.playerSprite;
        bowlerNameText.SetText(bowler.playerName);
        bowlerAbilityText.SetText(bowler.SpecialAbility);
        bowlerBowlingPowerText.SetText(bowler.BowlingPower.ToString());
    }

    public void LoadBatsman(PlayerData data, PlayerLineupView batsmanView)
    {
        if (data == null || batsmanView == null) return;

        batsmanView.SetCurrentPlayerIndicator(true);
        batsmanImage.sprite = data.playerSprite;
        batsmanNameText.SetText(data.playerName);
        batsmanAbilityText.SetText(data.SpecialAbility);
        battingPowerText.SetText(data.BattingPower.ToString());
        defenceText.SetText(data.Defense.ToString());
    }

    public void UpdateCurrentBatsman(PlayerDataDuringMatch data)
    {
        if (data == null) return;

        batsmanImage.sprite = data.playerSprite;
        batsmanNameText.SetText(data.playerName);
        batsmanAbilityText.SetText(data.SpecialAbility);
        battingPowerText.SetText(data.BattingPower.ToString());
        defenceText.SetText(data.Defense.ToString());
    }

    public void UpdateUIDuringBattle(PlayerLineupView batsmanView, PlayerDataDuringMatch runtimeData)
    {
        runtimeData.UpdatePlayerDataDuringMatch(runtimeData.Defense, runtimeData.BattingPower, runtimeData.BowlingPower);
        UpdateCurrentBatsman(runtimeData);
        batsmanView.UpdateDefense(runtimeData.Defense);
        batsmanView.SetCurrentPlayerIndicator(true);
    }

    public void HandleBatsmanOut(PlayerLineupView view)
    {
        if (view != null)
            view.MarkOut();

        if (defenceText != null)
            defenceText.SetText("OUT");
    }

    public void DefenceReducedTextEffect(string value)
    {

        float duration = 0.8f;
        float moveY = 50f;

        defenceText.text = "-"+value;

        RectTransform rect = defenceText.rectTransform;

        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0, moveY);

        Color startColor = defenceText.color;
        Color damageColor = new Color(1f, 0.1f, 0.1f);

        startColor.a = 1f;
        defenceText.color = damageColor;

        Sequence seq = DOTween.Sequence();

        seq.Join(rect.DOAnchorPos(endPos, duration).SetEase(Ease.OutQuad));
        defenceText.DOColor(new Color(defenceText.color.r, defenceText.color.g, defenceText.color.b, 0f), duration);

        seq.OnComplete(() =>
        {
            rect.anchoredPosition = startPos;

            Color c = defenceText.color;
            c.a = 0f;
            defenceText.color = c;
        });
    }

    public void DefenseGainedTextEffect(string value)
    {

        float duration = 0.8f;
        float moveY = 50f;

        defenceText.text = "+" + value;

        RectTransform rect = defenceText.rectTransform;

        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0, moveY);

        Color startColor = defenceText.color;
        Color damageColor = new Color(1f, 0.84f, 0f);


        startColor.a = 1f;
        defenceText.color = damageColor;

        Sequence seq = DOTween.Sequence();

        seq.Join(rect.DOAnchorPos(endPos, duration).SetEase(Ease.OutQuad));
        defenceText.DOColor(new Color(defenceText.color.r, defenceText.color.g, defenceText.color.b, 0f), duration);

        seq.OnComplete(() =>
        {
            rect.anchoredPosition = startPos;

            Color c = defenceText.color;
            c.a = 0f;
            defenceText.color = c;
        });
    }

    public void BattingPowerGainedTextEffect(string value)
    {

        float duration = 0.8f;
        float moveY = 50f;

        battingPowerTextEffects.text = "+" + value;

        RectTransform rect = battingPowerTextEffects.rectTransform;

        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0, moveY);

        Color startColor = battingPowerTextEffects.color;
        Color damageColor = new Color(1f, 0.84f, 0f);


        startColor.a = 1f;
        battingPowerTextEffects.color = damageColor;

        Sequence seq = DOTween.Sequence();

        seq.Join(rect.DOAnchorPos(endPos, duration).SetEase(Ease.OutQuad));
        battingPowerTextEffects.DOColor(new Color(battingPowerTextEffects.color.r, battingPowerTextEffects.color.g, battingPowerTextEffects.color.b, 0f), duration);

        seq.OnComplete(() =>
        {
            rect.anchoredPosition = startPos;

            Color c = battingPowerTextEffects.color;
            c.a = 0f;
            battingPowerTextEffects.color = c;
        });
    }

    private void PlayFloatingTextEffect(TMPro.TMP_Text textComp, string value, bool isPositive, Color color)
    {
        float duration = 0.8f;
        float moveY = 50f;

        textComp.text = (isPositive ? "+" : "-") + value;

        RectTransform rect = textComp.rectTransform;

        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0, moveY);

        textComp.color = color;

        Sequence seq = DOTween.Sequence();

        seq.Join(rect.DOAnchorPos(endPos, duration).SetEase(Ease.OutQuad));
        seq.Join(textComp.DOColor(new Color(color.r, color.g, color.b, 0f), duration));

        seq.OnComplete(() =>
        {
            rect.anchoredPosition = startPos;

            Color c = textComp.color;
            c.a = 0f;
            textComp.color = c;
        });
    }

}
