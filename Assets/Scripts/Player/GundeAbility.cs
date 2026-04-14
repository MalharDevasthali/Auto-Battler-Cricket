using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Gunde Ability")]
public class GundeAbility : PlayerAbility
{
 
    private BattleView battleView;
    private PlayerLineupView playerLineupView;


    public override void Init(BattleView battleView, PlayerLineupView playerLineupView)
    {

        this.battleView = battleView;
        this.playerLineupView = playerLineupView;


        Debug.Log("Gunde Ability Got Subscribed");
    }
    public override void EventUnSubscribe()
    {
   
        Debug.Log("Gunde Ability Got Unsubscribed");
    }
    public override void ProcessAbility(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runsOnCurrentBall, bool wicketFallen)
    {
        Debug.Log("Processing Gunde Ability - Runs: " + runsOnCurrentBall);
        OnComesToBat(batsmanData, bowlerData);
    }

    private void OnComesToBat(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData)
    {
        bowlerData.BowlingPower = bowlerData.BowlingPower - 1;

        bowlerData.UpdatePlayerDataDuringMatch(bowlerData.Defense, bowlerData.BattingPower, bowlerData.BowlingPower);
        battleView.BowlingPowerReducedTextEffect(1.ToString());
        battleView.UpdateUIDuringBattle(playerLineupView, batsmanData, bowlerData);
    }

}