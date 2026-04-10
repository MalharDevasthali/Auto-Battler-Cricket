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
    private EventService eventService;

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
        eventService = ServiceLocator.Instance.EventService;

        batsmen = lineupHolder.GetPlayerLineupList();
        UpdateScoreUI();
        LoadBowlerUI();
        LoadBatsmanUI(batsmen[0].GetData(), batsmen[0]);
    }
    public async void StartMatch()
    {
       
        startMatchButton.enabled = false;

        ResetMatch();

        int currentBatsmanIndex = 0;
       

        PlayerLineupView batsmanView = batsmen[currentBatsmanIndex];
        PlayerData batsmanData = batsmanView.GetData();
        PlayerDataDuringMatch runtimeData = new PlayerDataDuringMatch(batsmanData);
        runtimeData.runTimeAbility?.Init();

      
        for (int ball = 1; ball <= 6; ball++)
        {
            if (currentBatsmanIndex >= batsmen.Count) break;
            if (this == null) break;

            UpdateUIDuringBattle(batsmanView, runtimeData);
            await Task.Delay((int)(ballDelay * 1000));
           
            PlayBall(ball, batsmanView, runtimeData);
           
            UpdateUIDuringBattle(batsmanView, runtimeData);

            if (runtimeData.Defense <= 0)
            {  
                wickets++;
                currentBatsmanIndex++;
               
                UpdateScoreUI();
                HandleBatsmanOut(batsmanView, runtimeData);
                runtimeData.runTimeAbility.EventUnSubscribe();
                await Task.Delay((int)(ballDelay * 1000));

             
                if (currentBatsmanIndex < batsmen.Count && ball < 6) 
                {
                    batsmanView.SetCurrentPlayerIndicator(false);
                    BringNewPlayer(currentBatsmanIndex , out batsmanView, out batsmanData, out runtimeData);
                }
            }
        }

        Debug.Log($"Over finished. Total Runs: {totalRuns}, Wickets: {wickets}");
        
        startMatchButton.enabled = true;
    }

    private void UpdateUIDuringBattle( PlayerLineupView batsmanView, PlayerDataDuringMatch runtimeData)
    {
        runtimeData.UpdatePlayerDataDuringMatch(runtimeData.Defense, runtimeData.BattingPower, runtimeData.BowlingPower);
        UpdateCurrentBatsmanCard(runtimeData);
        batsmanView.UpdateDefense(runtimeData.Defense);
        batsmanView.SetCurrentPlayerIndicator(true);
    }

    private void BringNewPlayer(int currentBatsmanIndex, out PlayerLineupView batsmanView, out PlayerData batsmanData, out PlayerDataDuringMatch runtimeData)
    {
        batsmanView = batsmen[currentBatsmanIndex];
        batsmanData = batsmanView.GetData();
        runtimeData = new PlayerDataDuringMatch(batsmanData);
        batsmanView.SetCurrentPlayerIndicator(true);
    }

    private void ResetMatch()
    {
        batsmen = lineupHolder.GetPlayerLineupList();
        totalRuns = 0;
        wickets = 0;
        UpdateScoreUI();
        lineupHolder.ResetTeamLineUp();
        LoadBowlerUI();
        LoadBatsmanUI(batsmen[0].GetData(),batsmen[0]);
    
    }
    private void PlayBall(int ball,PlayerLineupView view, PlayerDataDuringMatch data)
    {
        ballText?.SetText($"Ball {ball}/6");
        Debug.Log($"{data.playerName} faces the bowler.");
        data.Defense = UpdateDefence(data.Defense);
        view.UpdateDefense(data.Defense);
     
        
        int runs = data.BattingPower;
        if (data.Defense > 0)
        {
            totalRuns += runs;
            OnRunsScored(data,runs);
        }
        Debug.Log($"{data.playerName} scores {runs} runs.");
        UpdateScoreUI();
    }

    private int UpdateDefence(int defense)
    {
        return Mathf.Max(0, defense - bowler.BowlingPower);
    }

    private void HandleBatsmanOut(PlayerLineupView view, PlayerDataDuringMatch batsmanWhoGotOut)
    {
        Debug.Log($"{batsmanWhoGotOut.playerName} is OUT");
        OnWhicketFallen(batsmanWhoGotOut);
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

    private void UpdateCurrentBatsmanCard(PlayerDataDuringMatch data)
    {
        if (data == null) return;

       batsmanImage.sprite = data.playerSprite;
       batsmanNameText.SetText(data.playerName);
       batsmanAbilityText.SetText(data.SpecialAbility);
       battingPowerText.SetText(data.BattingPower.ToString());
       defenceText.SetText(data.Defense.ToString());
    }
    private void LoadBatsmanUI(PlayerData data , PlayerLineupView batsmanview)
    {
        batsmanview.SetCurrentPlayerIndicator(true);
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

    private void OnRunsScored(PlayerDataDuringMatch player, int runs)
    {
        eventService.RaiseRunsScored(player, runs);
    }

    private void OnWhicketFallen(PlayerDataDuringMatch batsmanWhoGotOut)
    {
        eventService.RaiseWicketFallen(batsmanWhoGotOut);
    }
}

