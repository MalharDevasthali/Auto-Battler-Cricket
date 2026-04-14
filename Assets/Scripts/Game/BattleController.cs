using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerData bowlerData; // assign in inspector
    [SerializeField] private TeamLineupUIHolder lineupHolder; // assign in inspector
    [SerializeField] private BattleView battleView;
    [SerializeField] private AbilityQueueSystem abilityQueueSystem;

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
    private int runsOnCurrentBall = 0;
    private bool wicketFalledOnCurrentBall = false;

    private PlayerLineupView currentBatsmanView;
    private PlayerData batsmanData;
    private PlayerDataDuringMatch currentBatsmanDataDuringMatch;
    private List<PlayerDataDuringMatch> allBatsmanDataDuringMatch = new List<PlayerDataDuringMatch>(6);
    private PlayerDataDuringMatch currentBowlerDataDuringMatch;


    private void Start()
    {

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
        wicketFalledOnCurrentBall = false;

        for (int ball = 1; ball <= 6; ball++)
        {
            if (currentBatsmanIndex >= batsmen.Count) break;
            if (this == null) break;
            wicketFalledOnCurrentBall = false;

            battleView.UpdateUIDuringBattle(currentBatsmanView, currentBatsmanDataDuringMatch,currentBowlerDataDuringMatch);
            await Task.Delay((int)(ballDelay * 1000));


            ServiceLocator.Instance.SoundService.PlaySound(ballBowledSound);
            await Task.Delay(1000);

            PlayBall(ball, currentBatsmanView, currentBatsmanDataDuringMatch);
           
            battleView.UpdateUIDuringBattle(currentBatsmanView, currentBatsmanDataDuringMatch,currentBowlerDataDuringMatch);

            if (currentBatsmanDataDuringMatch.Defense <= 0)
            {
                await playWicketSound();

                wickets++;
                currentBatsmanIndex++;
                wicketFalledOnCurrentBall = true;


                battleView.UpdateScore(totalRuns, wickets);
                UpdateUIAfterWicket(currentBatsmanView, currentBatsmanDataDuringMatch);

                await Task.Delay((int)(ballDelay * 1000));

                if (currentBatsmanIndex < batsmen.Count && currentBall < 6)
                {
                    currentBatsmanView.SetCurrentPlayerIndicator(false);

                    BringNewPlayer(currentBatsmanIndex, out currentBatsmanView, out batsmanData, out currentBatsmanDataDuringMatch);
                    battleView.UpdateUIDuringBattle(currentBatsmanView, currentBatsmanDataDuringMatch, currentBowlerDataDuringMatch);
                }
            }
            else
            {
                await playBallHitSound();
                battleView.UpdateUIDuringBattle(currentBatsmanView, currentBatsmanDataDuringMatch, currentBowlerDataDuringMatch);
            }

            await processPlayerAbilities(currentBatsmanDataDuringMatch, currentBowlerDataDuringMatch, totalRuns, wickets, runsOnCurrentBall, ball);
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
        wicketFalledOnCurrentBall = false;
        battleView.UpdateUIDuringBattle(currentBatsmanView, currentBatsmanDataDuringMatch, currentBowlerDataDuringMatch );
        await Task.Delay((int)(ballDelay * 1000));
        ServiceLocator.Instance.SoundService.PlaySound(ballBowledSound);
        await Task.Delay((int)(ballDelay * 1000));

        PlayBall(currentBall, currentBatsmanView, currentBatsmanDataDuringMatch);

        if (currentBatsmanDataDuringMatch.Defense <= 0)
        {
            await handleWicketFall();
        }
        else
        {
            await playBallHitSound();
            battleView.UpdateUIDuringBattle(currentBatsmanView, currentBatsmanDataDuringMatch, currentBowlerDataDuringMatch);
        }

        await processPlayerAbilities(currentBatsmanDataDuringMatch,currentBowlerDataDuringMatch,totalRuns,wickets,runsOnCurrentBall,currentBall);
        
        currentBall++;
        if (currentBall > 6 || currentBatsmanIndex >= batsmen.Count)
        {
            Debug.Log($"Match is finished. Total Runs: {totalRuns}, Wickets: {wickets}");
        }
        battleView.SetPlayInteractable(true);
    }

    private async Task handleWicketFall()
    {
        await playWicketSound();

        wickets++;
        currentBatsmanIndex++;
        wicketFalledOnCurrentBall = true;

        battleView.UpdateScore(totalRuns, wickets);
        UpdateUIAfterWicket(currentBatsmanView, currentBatsmanDataDuringMatch);

        await Task.Delay((int)(ballDelay * 1000));

        if (currentBatsmanIndex < batsmen.Count && currentBall < 6)
        {
            currentBatsmanView.SetCurrentPlayerIndicator(false);

            BringNewPlayer(currentBatsmanIndex, out currentBatsmanView, out batsmanData, out currentBatsmanDataDuringMatch);
            battleView.UpdateUIDuringBattle(currentBatsmanView, currentBatsmanDataDuringMatch, currentBowlerDataDuringMatch);
        }
  
    }

    private void SetPlayersData()
    {
        if (currentBall == 1 && currentBatsmanDataDuringMatch == null)
        {
            ResetMatch();

            currentBatsmanIndex = 0;

            currentBatsmanView = batsmen[currentBatsmanIndex];
            batsmanData = currentBatsmanView.GetData();

            for (int i = 0; i < batsmen.Count; i++)
            {
                allBatsmanDataDuringMatch.Add(new PlayerDataDuringMatch(batsmen[i].GetData()));
            }
           
            currentBatsmanDataDuringMatch = allBatsmanDataDuringMatch[currentBatsmanIndex];
            currentBatsmanDataDuringMatch.playerAbilityDuringMatch?.Init(battleView, currentBatsmanView,abilityQueueSystem);

            currentBowlerDataDuringMatch = new PlayerDataDuringMatch(bowlerData);
            currentBowlerDataDuringMatch.playerAbilityDuringMatch?.Init(battleView, currentBatsmanView, abilityQueueSystem);
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

    private void BringNewPlayer(int currentBatsmanIndex, out PlayerLineupView batsmanView, out PlayerData batsmanData, out PlayerDataDuringMatch currentBatsmanDataDuringMatch)
    {
        batsmanView = batsmen[currentBatsmanIndex];
        batsmanData = batsmanView.GetData();
        currentBatsmanDataDuringMatch = allBatsmanDataDuringMatch[currentBatsmanIndex];
        currentBatsmanDataDuringMatch.playerAbilityDuringMatch?.Init(battleView,batsmanView, abilityQueueSystem);
        batsmanView.SetCurrentPlayerIndicator(true);
        currentBowlerDataDuringMatch.playerAbilityDuringMatch?.SetCurrentBatsmanView(batsmanView);
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
    private  void PlayBall(int ball,PlayerLineupView batsmanView, PlayerDataDuringMatch batsmanDataDuringMatch)
    {
        battleView.UpdateBallText(ball);
        Debug.Log($"{batsmanDataDuringMatch.playerName} faces the bowler.");
        batsmanDataDuringMatch.Defense = reduceDefence(batsmanDataDuringMatch.Defense);
       
        battleView.DefenceReducedTextEffect(currentBowlerDataDuringMatch.BowlingPower.ToString());
        batsmanView.UpdatePlayerView(batsmanDataDuringMatch.Defense,batsmanDataDuringMatch.BattingPower);

        runsOnCurrentBall = 0;
        if (batsmanDataDuringMatch.Defense > 0)
        {
            runsOnCurrentBall = batsmanDataDuringMatch.BattingPower;
            totalRuns += runsOnCurrentBall;
            batsmanDataDuringMatch.AddRunsToIndivisual(runsOnCurrentBall);
            batsmanView.UpdateIndivisualRuns(batsmanDataDuringMatch.playerRunsDuringMatch);
        }
        Debug.Log($"{batsmanDataDuringMatch.playerName} scores {runsOnCurrentBall} runs.");
        battleView.UpdateScore(totalRuns, wickets);
    }

    private int reduceDefence(int defense)
    {
        return Mathf.Max(0, defense - currentBowlerDataDuringMatch.BowlingPower);
    }

    private void UpdateUIAfterWicket(PlayerLineupView view, PlayerDataDuringMatch batsmanWhoGotOut)
    {
        Debug.Log($"{batsmanWhoGotOut.playerName} is OUT");
        battleView.HandleBatsmanOut(view);
    }

    private async Task processPlayerAbilities(PlayerDataDuringMatch batsmanDataDuringMatch , PlayerDataDuringMatch bowlerDataDuringMatch,int totalrun , int totalWickets, int runsOnCurrentBall , int currentBall)
    {

        for (int i = currentBatsmanIndex; i < allBatsmanDataDuringMatch.Count; i++)
        {       
            if(allBatsmanDataDuringMatch[i].playerAbilityDuringMatch != null)
                await allBatsmanDataDuringMatch[i].playerAbilityDuringMatch.ProcessAbility(batsmanDataDuringMatch, bowlerDataDuringMatch, runsOnCurrentBall,wicketFalledOnCurrentBall);    
        }
            await bowlerDataDuringMatch.playerAbilityDuringMatch.ProcessAbility(batsmanDataDuringMatch, bowlerDataDuringMatch, runsOnCurrentBall, wicketFalledOnCurrentBall);
            await abilityQueueSystem.WaitForAllAbilitiesAsync();
        
    }
}

