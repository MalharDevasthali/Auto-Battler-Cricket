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
    private UIService uiService;

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
    [SerializeField] private float newBatsmanDelay = 1.0f; // extra delay when new batsman comes in

    private List<PlayerLineupView> batsmen = new List<PlayerLineupView>();
    private int totalRuns = 0;
    private int wickets = 0;

    private void Start()
    {
        uiService = ServiceLocator.Instance.UIService;
 
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
        var batsmanView = batsmen[currentBatsmanIndex];
        var batsmanData = batsmanView.GetData();
        
        PlayerDataDuringMatch runtimeData = new PlayerDataDuringMatch(batsmanData, currentDefense);
        ServiceLocator.Instance.UIService.UpdateUIDuringMatch(runtimeData);
        
        await Task.Delay((int)(ballDelay * 1000));

        for (int ball = 1; ball <= 6; ball++)
        {
           
            if (currentBatsmanIndex >= batsmen.Count) break;

            ballText?.SetText($"Ball {ball}/6");

            batsmanView = batsmen[currentBatsmanIndex];
            batsmanData = batsmanView.GetData();

            runtimeData = new PlayerDataDuringMatch(batsmanData, currentDefense);
           
            runtimeData.UpdatePlayerDataDuringMatch(currentDefense, batsmanData.BattingPower, batsmanData.BowlingPower);  
            ServiceLocator.Instance.UIService.UpdateUIDuringMatch(runtimeData);
            
            PlayBall(batsmanView, batsmanData, ref currentDefense);

            runtimeData.UpdatePlayerDataDuringMatch(currentDefense, batsmanData.BattingPower, batsmanData.BowlingPower);
            ServiceLocator.Instance.UIService.UpdateUIDuringMatch(runtimeData);

            if (currentDefense <= 0)
            {
                wickets++;
                HandleBatsmanOut(batsmanView, batsmanData);
                currentBatsmanIndex++;
                if (currentBatsmanIndex < batsmen.Count)
                    currentDefense = GetDefenseForBatsman(currentBatsmanIndex);

                // extra delay to show new batsman's stats before next ball
                if (currentBatsmanIndex < batsmen.Count)
                    await Task.Delay((int)(newBatsmanDelay * 1000));

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
        runtimeDefense = UpdateDefence(runtimeDefense);
        view.UpdateDefense(runtimeDefense);
        int runs = data.BattingPower;
        totalRuns += runs;
        Debug.Log($"{data.playerName} scores {runs} runs.");
        UpdateScoreUI();
    }

    private int UpdateDefence(int defense)
    {
        return Mathf.Max(0, defense - bowler.BowlingPower);
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

