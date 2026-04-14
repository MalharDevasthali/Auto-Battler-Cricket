using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Gunde Ability")]
public class GundeAbility : PlayerAbility
{
    private EventService eventService;

    private BattleView battleView;
    private PlayerLineupView playerLineupView;


    public override void Init(BattleView battleView, PlayerLineupView playerLineupView)
    {
        eventService = ServiceLocator.Instance.EventService;
        this.battleView = battleView;
        this.playerLineupView = playerLineupView;

        eventService.OnComesToBat += OnComesToBat;
        Debug.Log("Gunde Ability Got Subscribed");
    }
    public override void EventUnSubscribe()
    {
        eventService.OnComesToBat -= OnComesToBat;
        Debug.Log("Gunde Ability Got Unsubscribed");
    }
    public override void ProcessAbility(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runsOnCurrentBall)
    {
        Debug.Log("Processing Gunde Ability - Runs: " + runsOnCurrentBall);
    }
    private void OnComesToBat(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, float abilityDelay)
    {
        bowlerData.BowlingPower = bowlerData.BowlingPower - 1;

        bowlerData.UpdatePlayerDataDuringMatch(bowlerData.Defense, bowlerData.BattingPower, bowlerData.BowlingPower);
        battleView.BowlingPowerReducedTextEffect(1.ToString());
        battleView.UpdateUIDuringBattle(playerLineupView, batsmanData, bowlerData);
    }
}