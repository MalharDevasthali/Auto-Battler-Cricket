using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shubhankar Ability")]
public class ShubhankarAbility : PlayerAbility
{
    private EventService eventService;

    private BattleView battleView;
    private PlayerLineupView playerLineupView;


    public override void Init(BattleView battleView, PlayerLineupView playerLineupView)
    {
        eventService = ServiceLocator.Instance.EventService;
        this.battleView = battleView;
        this.playerLineupView = playerLineupView;

        eventService.OnWicketFallen += OnWicketFallen;
        Debug.Log("Shubhankar Ability Got Subscribed");
    }
    public override void EventUnSubscribe()
    {
        eventService.OnWicketFallen -= OnWicketFallen;
        Debug.Log("Shubhankar Ability Got Unsubscribed");
    }

    public override void ProcessAbility(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runsOnCurrentBall)
    {
        Debug.Log("Processing Shubhankar Ability - Runs: " + runsOnCurrentBall);
    }

    private void OnWicketFallen(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData,float abilityDelay)
    {
        bowlerData.BowlingPower = bowlerData.BowlingPower + 1;

        bowlerData.UpdatePlayerDataDuringMatch(bowlerData.Defense, bowlerData.BattingPower, bowlerData.BowlingPower);
        battleView.BowlingPowerGainedTextEffect(1.ToString());
        battleView.UpdateUIDuringBattle(playerLineupView,batsmanData,bowlerData);
    }
}