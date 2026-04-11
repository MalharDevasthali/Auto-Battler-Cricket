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

    public void UpdateDuringBattle(PlayerLineupView batsmanView, PlayerDataDuringMatch runtimeData)
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
}
