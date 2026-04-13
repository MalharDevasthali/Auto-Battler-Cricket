using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class BattleController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerData bowlerData; // assign in inspector
    [SerializeField] private TeamLineupUIHolder lineupHolder; // assign in inspector
    [SerializeField] private BattleView battleView;
    private EventService eventService;

    [Header("Sounds")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip wicketSound;
    [SerializeField] private AudioClip ballBowledSound;
    [SerializeField] private AudioClip crowdCheeringSound;

    [Header("Simulation")]
    [SerializeField] private float ballDelay = 1.0f; // seconds between balls

    private List<PlayerLineupView> batsmen = new List<PlayerLineupView>();
    private int totalRuns = 0;
    private int wickets = 0;


    private int currentBatsmanIndex = 0;
    private int currentBall = 1;

    private PlayerLineupView batsmanView;
    private PlayerData batsmanData;
    private PlayerDataDuringMatch batsmanDataDuringMatch;
    private PlayerDataDuringMatch bowlerDataDuringMatch;

    private void Start()
    {
        eventService = ServiceLocator.Instance.EventService;

        batsmen = lineupHolder.GetPlayerLineupList();
        battleView.UpdateScore(totalRuns, wickets);
        battleView.LoadBowler(bowlerData);
        battleView.LoadBatsman(batsmen[0].GetData(), batsmen[0]);
    }
    public async void StartMatch()
    {
       
        battleView.SetStartMatchInteractable(false);
        if (this == null) return;
        if (currentBatsmanIndex >= batsmen.Count) return;

        SetPlayersData();


        for (int ball = 1; ball <= 6; ball++)
        {
            if (currentBatsmanIndex >= batsmen.Count) break;
            if (this == null) break;

            battleView.UpdateUIDuringBattle(batsmanView, batsmanDataDuringMatch);
            await Task.Delay((int)(ballDelay * 1000));


            ServiceLocator.Instance.SoundService.PlaySound(ballBowledSound);
            await Task.Delay(1000);

            PlayBall(ball, batsmanView, batsmanDataDuringMatch);
           
            battleView.UpdateUIDuringBattle(batsmanView, batsmanDataDuringMatch);

            if (batsmanDataDuringMatch.Defense <= 0)
            {
                await playWicketSound();

                wickets++;
                currentBatsmanIndex++;

                OnWhicketFallen(batsmanDataDuringMatch);
                battleView.UpdateScore(totalRuns, wickets);
                UpdateUIAfterWicket(batsmanView, batsmanDataDuringMatch);

                batsmanDataDuringMatch.runTimeAbility?.EventUnSubscribe();

                await Task.Delay((int)(ballDelay * 1000));

                if (currentBatsmanIndex < batsmen.Count && currentBall < 6)
                {
                    batsmanView.SetCurrentPlayerIndicator(false);

                    BringNewPlayer(currentBatsmanIndex, out batsmanView, out batsmanData, out batsmanDataDuringMatch);
                    battleView.UpdateUIDuringBattle(batsmanView, batsmanDataDuringMatch);
                }
            }
            else
            {
                await playBallHitSound();
                battleView.UpdateUIDuringBattle(batsmanView, batsmanDataDuringMatch);
            }
        }

        Debug.Log($"Over finished. Total Runs: {totalRuns}, Wickets: {wickets}");
        
        battleView.SetStartMatchInteractable(true);
    }

    public async void PlayNextBall()
    {
        battleView.SetPlayInteractable(false);

        if (this == null) return;
        if (currentBall > 6) return;
        if (currentBatsmanIndex >= batsmen.Count) return;

        SetPlayersData();
        battleView.UpdateUIDuringBattle(batsmanView, batsmanDataDuringMatch);

        await Task.Delay((int)(ballDelay * 1000));

        ServiceLocator.Instance.SoundService.PlaySound(ballBowledSound);

        await Task.Delay((int)(ballDelay * 1000));

        PlayBall(currentBall, batsmanView, batsmanDataDuringMatch);

        if (batsmanDataDuringMatch.Defense <= 0)
        {
            await playWicketSound();

            wickets++;
            currentBatsmanIndex++;
            
            OnWhicketFallen(batsmanDataDuringMatch);
            battleView.UpdateScore(totalRuns, wickets);
            UpdateUIAfterWicket(batsmanView, batsmanDataDuringMatch);

            batsmanDataDuringMatch.runTimeAbility?.EventUnSubscribe();

            await Task.Delay((int)(ballDelay * 1000));

            if (currentBatsmanIndex < batsmen.Count && currentBall < 6)
            {
                batsmanView.SetCurrentPlayerIndicator(false);

                BringNewPlayer(currentBatsmanIndex, out batsmanView, out batsmanData, out batsmanDataDuringMatch);
                battleView.UpdateUIDuringBattle(batsmanView, batsmanDataDuringMatch);
            }
        }
        else
        {
            await playBallHitSound();
            battleView.UpdateUIDuringBattle(batsmanView, batsmanDataDuringMatch);
        }

        currentBall++;

        if (currentBall > 6 || currentBatsmanIndex >= batsmen.Count)
        {
            Debug.Log($"Over finished. Total Runs: {totalRuns}, Wickets: {wickets}");
        }

        battleView.SetPlayInteractable(true);
    }

    private void SetPlayersData()
    {
        if (currentBall == 1 && batsmanDataDuringMatch == null)
        {
            ResetMatch();

            currentBatsmanIndex = 0;

            batsmanView = batsmen[currentBatsmanIndex];
            batsmanData = batsmanView.GetData();
            batsmanDataDuringMatch = new PlayerDataDuringMatch(batsmanData);
            batsmanDataDuringMatch.runTimeAbility?.Init(battleView, batsmanView);

            bowlerDataDuringMatch = new PlayerDataDuringMatch(bowlerData);
            bowlerDataDuringMatch.runTimeAbility?.Init(battleView,null);
        }
    }

    private async Task playBallHitSound()
    {
        ServiceLocator.Instance.SoundService.PlaySound(hitSound);
        await Task.Delay(100);
        ServiceLocator.Instance.SoundService.PlaySound(crowdCheeringSound,0.5f);
    }

    private async Task playWicketSound()
    {
        ServiceLocator.Instance.SoundService.PlaySound(wicketSound);
        await Task.Delay(100);
        ServiceLocator.Instance.SoundService.PlaySound(crowdCheeringSound);
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
        battleView.UpdateScore(totalRuns, wickets);
        lineupHolder.ResetTeamLineUp();
        battleView.LoadBowler(bowlerData);
        battleView.LoadBatsman(batsmen[0].GetData(),batsmen[0]);
    
    }
    private  void PlayBall(int ball,PlayerLineupView view, PlayerDataDuringMatch data)
    {
        battleView.UpdateBallText(ball);
        Debug.Log($"{data.playerName} faces the bowler.");
        data.Defense = reduceDefence(data.Defense);
       
        battleView.DefenceReducedTextEffect(bowlerDataDuringMatch.BowlingPower.ToString());
        view.UpdateDefense(data.Defense);

        int runsOnThisBall = data.BattingPower;
        if (data.Defense > 0)
        {
            totalRuns += runsOnThisBall;
            data.SetIndivisualRunsScored(runsOnThisBall);
            OnRunsScored(data,data.playerRunsDuringMatch);
            view.UpdateIndivisualRuns(data.playerRunsDuringMatch);
        }
        Debug.Log($"{data.playerName} scores {runsOnThisBall} runs.");
        battleView.UpdateScore(totalRuns, wickets);
    }

    private int reduceDefence(int defense)
    {
        return Mathf.Max(0, defense - bowlerDataDuringMatch.BowlingPower);
    }

    private void UpdateUIAfterWicket(PlayerLineupView view, PlayerDataDuringMatch batsmanWhoGotOut)
    {
        Debug.Log($"{batsmanWhoGotOut.playerName} is OUT");
        battleView.HandleBatsmanOut(view);
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

