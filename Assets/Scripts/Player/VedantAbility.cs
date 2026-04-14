using System.Threading.Tasks;
using UnityEngine;
using static Unity.Collections.Unicode;

[CreateAssetMenu(menuName = "Abilities/Vedant Ability")]
public class VedantAbility : PlayerAbility
{

    public int defenceBoostAfterAbility = 1;
    public int battingPowerBoostAfterAbility = 1;

    private EventService eventService;

    private BattleView battleView;
    private PlayerLineupView playerLineupView;

    private int lastProcessRuns = 0;

    public override void Init(BattleView battleView, PlayerLineupView playerLineupView)
    {
        eventService = ServiceLocator.Instance.EventService;
        this.battleView = battleView;
        this.playerLineupView = playerLineupView;

        eventService.OnRunsScored += OnRunsScored;
        Debug.Log("Vedant Ability Got Subscribed");
    }
    public override void EventUnSubscribe()
    {
        eventService.OnRunsScored -= OnRunsScored;
        Debug.Log("Vedant Ability Got Unsubscribed");
    }

    public override void ProcessAbility(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runsOnCurrentBall)
    {
        Debug.Log("Processing Vedant Ability - Runs: " + runsOnCurrentBall);
        if (lastProcessRuns < 4 && runsOnCurrentBall >= 4)
        {
          
            Debug.Log("Vedant Defence Ability Got Triggered,Runs: " + runsOnCurrentBall);
            batsmanData.Defense += defenceBoostAfterAbility;
            batsmanData.UpdatePlayerDataDuringMatch(batsmanData.Defense, batsmanData.BattingPower, batsmanData.BowlingPower);
            battleView.DefenseGainedTextEffect(defenceBoostAfterAbility.ToString());

            playerLineupView.UpdateDefense(batsmanData.Defense);
            battleView.UpdateUIDuringBattle(playerLineupView, batsmanData, bowlerData);
        }

        if (lastProcessRuns < 6 && runsOnCurrentBall >= 6)
        {
            Debug.Log("Vedant Batting Power Ability Got Triggered,Runs: " + runsOnCurrentBall);
            batsmanData.BattingPower += battingPowerBoostAfterAbility;
            batsmanData.UpdatePlayerDataDuringMatch(batsmanData.Defense, batsmanData.BattingPower, batsmanData.BowlingPower);
            battleView.BattingPowerGainedTextEffect(battingPowerBoostAfterAbility.ToString());

            playerLineupView.UpdateBattingPower(batsmanData.BattingPower);
            battleView.UpdateUIDuringBattle(playerLineupView, batsmanData, bowlerData);
        }
        lastProcessRuns = runsOnCurrentBall;
    }

    private async void OnRunsScored(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runs,float abilityDelay)
    {
       
        await Task.Delay(0);
    }
}