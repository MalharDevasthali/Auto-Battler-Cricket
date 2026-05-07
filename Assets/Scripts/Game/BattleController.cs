using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [Header("References")]
    
    [SerializeField] private LeaugeData[] leagueData;
    [SerializeField] private TeamLineupUIHolder lineupHolder;
    [SerializeField] private BattleView battleView;
    [SerializeField] private AbilityQueueSystem abilityQueueSystem;

    [Header("Sounds")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip wicketSound;
    [SerializeField] private AudioClip ballBowledSound;
    [SerializeField] private AudioClip crowdCheeringSound;

    [Header("Simulation")]
    [SerializeField] private float ballDelay = 1.0f; 

    private List<PlayerLineupView> battingTeamLineUp = new List<PlayerLineupView>();
    private PlayerData bowlingTeamData;

    private int totalRuns = 0;
    private int target = 0;
    private int wickets = 0;


    private int currentBatsmanIndex;
    private int currentBall;
    private int runsOnCurrentBall;
    private int currentInnings = 1;
    private bool wicketFalledOnCurrentBall = false;

    private PlayerLineupView currentBatsmanView;
    private PlayerData batsmanData;
    private PlayerDataDuringMatch currentBatsmanDataDuringMatch;
    private List<PlayerDataDuringMatch> allBatsmanDataDuringMatch = new List<PlayerDataDuringMatch>(6);
    private PlayerDataDuringMatch currentBowlerDataDuringMatch;


    private void Start()
    {
        InitializeMatch();
    }

    private void InitializeMatch()
    {
        totalRuns = 0;
        wickets = 0;
        currentBall = 1;
        currentBatsmanIndex = 0;

        currentBatsmanDataDuringMatch = null;
        allBatsmanDataDuringMatch.Clear();
        currentBowlerDataDuringMatch = null;
        
        
        Innings innings = ServiceLocator.Instance.GameService.GetCurrentInnings();
        battingTeamLineUp = lineupHolder.GetPlayerLineupList(innings);
        if (innings == Innings.Batting)
        {
            bowlingTeamData = ServiceLocator.Instance.GameService.GetCPUBowler();
        }
        else
        {
            bowlingTeamData = ServiceLocator.Instance.GameService.GetPlayerBowler();
        }

        SetBatsmanBowlerUI();
    }
   /* public async void StartMatch()
    {
       
        battleView.SetStartMatchInteractable(false);

        SetPlayersData();
        wicketFalledOnCurrentBall = false;

        for (int ball = 1; ball <= 6; ball++)
        {
            if (currentBatsmanIndex >= battingTeamLineUp.Count) break;
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

                if (currentBatsmanIndex < battingTeamLineUp.Count && currentBall < 6)
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
    }*/

    public async void PlayNextBall()
    {
        battleView.SetPlayInteractable(false);
        SetPlayersData();
        wicketFalledOnCurrentBall = false;
        battleView.UpdateUIDuringBattle(currentBatsmanView, currentBatsmanDataDuringMatch, currentBowlerDataDuringMatch);
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

        await processPlayerAbilities(currentBatsmanDataDuringMatch, currentBowlerDataDuringMatch, totalRuns, wickets, runsOnCurrentBall, currentBall);

        currentBall++;

        if (currentInnings == 2)
        {
            MatchWinCheck();
        }

        if (currentBall > 6 || currentBatsmanIndex >= battingTeamLineUp.Count)
        {
            battleView.SetPlayInteractable(false);
            
            if (currentInnings == 1)
            {
                processInningsEnd();
            }
            else if (currentInnings == 2)
            {
                MatchFinishedCheck();
            }
        }
        else
        {
            battleView.SetPlayInteractable(true);
        }
   
    }

    private void MatchWinCheck()
    {
        if (checkRunChasedSuccessfully())
        {
            battleView.SetPlayInteractable(false);
            if (ServiceLocator.Instance.GameService.GetCurrentInnings() == Innings.Batting)
            {
                Debug.Log("You Have Won!");
            }
            else
            {
                Debug.Log("CPU Have Won!");
            }
        }
    }
    private void MatchFinishedCheck()
    {
        if (checkRunChasedSuccessfully())
        {
            if (ServiceLocator.Instance.GameService.GetCurrentInnings() == Innings.Batting)
            {
                Debug.Log("You Have Won!");
            }
            else
            {
                Debug.Log("CPU Have Won!");
            }
        }
        else
        {
            if (ServiceLocator.Instance.GameService.GetCurrentInnings() == Innings.Bowling)
            {
                Debug.Log("You Have Won!");
            }
            else
            {
                Debug.Log("CPU Have Won!");
            }
        }
    }

    public void NextInningsButton()
    {
        Debug.Log($"Inning is finished. Total Runs: {totalRuns}, Wickets: {wickets}");
        currentInnings = 2;
        Innings currentInning = ServiceLocator.Instance.GameService.GetCurrentInnings();
        Innings nextInnings = currentInning == Innings.Batting ? Innings.Bowling : Innings.Batting;
        ServiceLocator.Instance.GameService.SetCurrentInnings(nextInnings);
        target = totalRuns;
        battleView.UpdateTarget(target);
        InitializeMatch();


        battleView.SetInningsButtonVisibility(false);
        battleView.SetPlayInteractable(true);
    }

    public void NextMatchButton()
    {

    }

    private void processInningsEnd()
    {
        battleView.SetInningsButtonVisibility(true);
    }
    private bool checkRunChasedSuccessfully()
    {
        if (totalRuns > target)
        {
            return true;
        }
        return false;
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

        if (currentBatsmanIndex < battingTeamLineUp.Count && currentBall < 6)
        {
            currentBatsmanView.SetCurrentPlayerIndicator(false);

            BringNewPlayer(currentBatsmanIndex, out currentBatsmanView, out currentBatsmanDataDuringMatch);
            battleView.UpdateUIDuringBattle(currentBatsmanView, currentBatsmanDataDuringMatch, currentBowlerDataDuringMatch);
        }
  
    }

    private void SetPlayersData()
    {
        if (currentBall == 1)
        {
            currentBatsmanIndex = 0;

            currentBatsmanView = battingTeamLineUp[currentBatsmanIndex];

            for (int i = 0; i < battingTeamLineUp.Count; i++)
            {
                allBatsmanDataDuringMatch.Add(new PlayerDataDuringMatch(battingTeamLineUp[i].GetData()));
            }

            currentBatsmanDataDuringMatch = allBatsmanDataDuringMatch[currentBatsmanIndex];
            currentBatsmanDataDuringMatch.playerAbilityDuringMatch?.Init(battleView, currentBatsmanView, abilityQueueSystem);

            currentBowlerDataDuringMatch = new PlayerDataDuringMatch(bowlingTeamData);
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

    private void BringNewPlayer(int currentBatsmanIndex, out PlayerLineupView batsmanView, out PlayerDataDuringMatch currentBatsmanDataDuringMatch)
    {
        batsmanView = battingTeamLineUp[currentBatsmanIndex];
        currentBatsmanDataDuringMatch = allBatsmanDataDuringMatch[currentBatsmanIndex];
        currentBatsmanDataDuringMatch.playerAbilityDuringMatch?.Init(battleView,batsmanView, abilityQueueSystem);
        batsmanView.SetCurrentPlayerIndicator(true);
        currentBowlerDataDuringMatch.playerAbilityDuringMatch?.SetCurrentBatsmanView(batsmanView);
    }

    private void SetBatsmanBowlerUI()
    {
        battleView.UpdateBallText(currentBall);
        battleView.UpdateScore(totalRuns, wickets);
        battleView.LoadBatsman(battingTeamLineUp[0].GetData(), battingTeamLineUp[0]);
        battleView.LoadBowler(bowlingTeamData);
        battleView.UpdateInningsUI();

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

