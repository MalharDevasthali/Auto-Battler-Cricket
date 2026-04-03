using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

// Simple battle simulation controller for 6 balls
// Assign a bowler via inspector (PlayerData ScriptableObject)
public class BattleController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerData bowler; // assign in inspector
    [SerializeField] private TeamLineupUIHolder lineupHolder; // assign in inspector

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText; // total score UI
    [SerializeField] private TextMeshProUGUI ballText; // current ball UI (e.g., "Ball 1/6")

    [Header("Bowler UI")]
    [SerializeField] private Image bowlerImage;
    [SerializeField] private TMPro.TextMeshProUGUI bowlerNameText;
    [SerializeField] private TMPro.TextMeshProUGUI bowlerDetailsText;
    [SerializeField] private TMPro.TextMeshProUGUI bowlerBowlingPowerText;
    [SerializeField] private Button startMatchButton;

    [Header("Simulation")]
    [SerializeField] private float ballDelay = 1.0f; // seconds between balls

    private List<PlayerLineupView> batsmen = new List<PlayerLineupView>();
    private int totalRuns = 0;
    private int wickets = 0;

    private void Start()
    {
        if (bowler == null)
        {
            Debug.LogError("BattleController: Bowler is not assigned in inspector");
            return;
        }

        if (lineupHolder == null)
        {
            Debug.LogError("BattleController: TeamLineupUIHolder is not assigned in inspector");
            return;
        }

        batsmen = lineupHolder.GetPlayerLineupList();
        UpdateScoreUI();
        LoadBowlerUI();
    }
    public async void StartOver()
    {
        startMatchButton.enabled = false;

        ResetMatch();

        int currentBatsmanIndex = 0;
        int currentDefense = GetDefenseForBatsman(currentBatsmanIndex);

        for (int ball = 1; ball <= 6; ball++)
        {
            if (currentBatsmanIndex >= batsmen.Count) break;

            ballText?.SetText($"Ball {ball}/6");

            var batsmanView = batsmen[currentBatsmanIndex];
            var batsmanData = batsmanView.GetData();

            PlayBall(batsmanView, batsmanData, ref currentDefense);

            if (currentDefense <= 0)
            {
                wickets++;
                HandleBatsmanOut(batsmanView, batsmanData);
                currentBatsmanIndex++;
                if (currentBatsmanIndex < batsmen.Count)
                    currentDefense = GetDefenseForBatsman(currentBatsmanIndex);
            }

            await Task.Delay((int)(ballDelay * 1000));
        }

        Debug.Log($"Over finished. Total Runs: {totalRuns}, Wickets: {wickets}");
        
        startMatchButton.enabled = true;
    }

    private void ResetMatch()
    {
        batsmen = lineupHolder.GetPlayerLineupList();
        totalRuns = 0;
        wickets = 0;
        UpdateScoreUI();
        lineupHolder.ResetTeamLineUp();
    }

    private int GetDefenseForBatsman(int index)
    {
        return batsmen[index].GetData().Defense;
    }
    private void PlayBall(PlayerLineupView view, PlayerData data, ref int runtimeDefense)
    {
        Debug.Log($"Ball: {data.playerName} faces the bowler.");
        runtimeDefense = ApplyBowlerToDefense(runtimeDefense);
        view.UpdateDefense(runtimeDefense);
        int runs = data.BattingPower;
        totalRuns += runs;
        Debug.Log($"{data.playerName} scores {runs} runs.");
        UpdateScoreUI();
    }

    private int ApplyBowlerToDefense(int defense)
    {
        return  defense - bowler.BowlingPower;
    }

    private void HandleBatsmanOut(PlayerLineupView view, PlayerData data)
    {
        Debug.Log($"{data.playerName} is OUT");
        view.MarkOut();
    }

    private void LoadBowlerUI()
    {   
          bowlerImage.sprite = bowler.playerSprite;
          bowlerNameText.SetText(bowler.playerName);
          bowlerDetailsText.SetText(bowler.SpecialAbility);
          bowlerBowlingPowerText.SetText(bowler.BowlingPower.ToString());

    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.SetText($"Score: {totalRuns}/{wickets}");
    }

}
