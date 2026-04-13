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
        Debug.Log("Vedant Ability Got Subscribed");
    }
    public override void EventUnSubscribe()
    {
        eventService.OnComesToBat -= OnComesToBat;
        Debug.Log("Vedant Ability Got Unsubscribed");
    }

    private void OnComesToBat(PlayerDataDuringMatch bowlerData)
    {
        bowlerData.BowlingPower = bowlerData.BowlingPower - 1;

        bowlerData.UpdatePlayerDataDuringMatch(bowlerData.Defense, bowlerData.BattingPower, bowlerData.BowlingPower);
        battleView.BowlingPowerReducedTextEffect(1.ToString());
        battleView.UpdateCurrentBowler(bowlerData);
    }
}