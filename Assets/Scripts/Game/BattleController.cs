using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class BattleController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerData bowler; // assign in inspector
    [SerializeField] private TeamLineupUIHolder lineupHolder; // assign in inspector
    private UIService uiService;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText; // total score UI
    [SerializeField] private TextMeshProUGUI ballText; // current ball UI (e.g., "Ball 1/6")
    [SerializeField] private Button startMatchButton;

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

    [Header("Simulation")]
    [SerializeField] private float ballDelay = 1.0f; // seconds between balls

    private List<PlayerLineupView> batsmen = new List<PlayerLineupView>();
    private int totalRuns = 0;
    private int wickets = 0;

    private void Start()
    {
        uiService = ServiceLocator.Instance.UIService;
 
        batsmen = lineupHolder.GetPlayerLineupList();
        UpdateScoreUI();
        LoadBowlerUI();
        LoadBatsmanUI(batsmen[0].GetData());
    }
    public async void StartOver()
    {
       
        startMatchButton.enabled = false;

        ResetMatch();

        int currentBatsmanIndex = 0;
        int currentDefense = GetDefenseForBatsman(currentBatsmanIndex);
        PlayerLineupView batsmanView = batsmen[currentBatsmanIndex];
        PlayerData batsmanData = batsmanView.GetData();
        PlayerDataDuringMatch runtimeData = new PlayerDataDuringMatch(batsmanData, currentDefense);

        for (int ball = 1; ball <= 6; ball++)
        {
           
            if (currentBatsmanIndex >= batsmen.Count) break;

          
            runtimeData.UpdatePlayerDataDuringMatch(currentDefense, batsmanData.BattingPower, batsmanData.BowlingPower);
            LoadBatsmanUI(runtimeData);
            batsmanView.UpdateDefense(currentDefense);
            
            await Task.Delay((int)(ballDelay * 1000));
            
            ballText?.SetText($"Ball {ball}/6");
            PlayBall(batsmanView, batsmanData, ref currentDefense);

            runtimeData.UpdatePlayerDataDuringMatch(currentDefense, batsmanData.BattingPower, batsmanData.BowlingPower);
            LoadBatsmanUI(runtimeData);
            batsmanView.UpdateDefense(currentDefense);

            if (currentDefense <= 0)
            {
                wickets++;
                UpdateScoreUI();
                HandleBatsmanOut(batsmanView, batsmanData);
                currentBatsmanIndex++;
                if (currentBatsmanIndex < batsmen.Count)
                {
                    BringNewPlayer(currentBatsmanIndex, out currentDefense, out batsmanView, out batsmanData, out runtimeData);
                }
                await Task.Delay((int)(ballDelay * 1000));
            }
        }

        Debug.Log($"Over finished. Total Runs: {totalRuns}, Wickets: {wickets}");
        
        startMatchButton.enabled = true;
    }

    private void BringNewPlayer(int currentBatsmanIndex, out int currentDefense, out PlayerLineupView batsmanView, out PlayerData batsmanData, out PlayerDataDuringMatch runtimeData)
    {
        currentDefense = GetDefenseForBatsman(currentBatsmanIndex);
        batsmanView = batsmen[currentBatsmanIndex];
        batsmanData = batsmanView.GetData();
        runtimeData = new PlayerDataDuringMatch(batsmanData, currentDefense);
    }

    private void ResetMatch()
    {
        batsmen = lineupHolder.GetPlayerLineupList();
        totalRuns = 0;
        wickets = 0;
        UpdateScoreUI();
        lineupHolder.ResetTeamLineUp();
        LoadBowlerUI();
        LoadBatsmanUI(batsmen[0].GetData());
    }

    private int GetDefenseForBatsman(int index)
    {
        return batsmen[index].GetData().Defense;
    }
    private void PlayBall(PlayerLineupView view, PlayerData data, ref int runtimeDefense)
    {
        Debug.Log($"{data.playerName} faces the bowler.");
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
        defenceText.SetText(defenceText.text = "OUT");
    }

    private void LoadBowlerUI()
    {   
          bowlerImage.sprite = bowler.playerSprite;
          bowlerNameText.SetText(bowler.playerName);
          bowlerAbilityText.SetText(bowler.SpecialAbility);
          bowlerBowlingPowerText.SetText(bowler.BowlingPower.ToString());
    }

    private void LoadBatsmanUI(PlayerDataDuringMatch data)
    {
        if (data == null) return;

       batsmanImage.sprite = data.playerSprite;
       batsmanNameText.SetText(data.playerName);
       batsmanAbilityText.SetText(data.SpecialAbility);
       battingPowerText.SetText(data.BattingPower.ToString());
       defenceText.SetText(data.Defense.ToString());
    }
    private void LoadBatsmanUI(PlayerData data)
    {
        if (data == null) return;

        batsmanImage.sprite = data.playerSprite;
        batsmanNameText.SetText(data.playerName);
        batsmanAbilityText.SetText(data.SpecialAbility);
        battingPowerText.SetText(data.BattingPower.ToString());
        defenceText.SetText(data.Defense.ToString());
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.SetText($"Score: {totalRuns}/{wickets}");
    }
}

