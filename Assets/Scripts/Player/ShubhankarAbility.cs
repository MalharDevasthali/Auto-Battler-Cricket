using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shubhankar Ability")]
public class ShubhankarAbility : PlayerAbility
{
    private AbilityQueueSystem eventService;

    private BattleView battleView;
    private PlayerLineupView playerLineupView;


    public override void Init(BattleView battleView, PlayerLineupView playerLineupView)
    {
     
        this.battleView = battleView;
        this.playerLineupView = playerLineupView;

    
        Debug.Log("Shubhankar Ability Got Subscribed");
    }
 

    public override void ProcessAbility(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runsOnCurrentBall, bool wicketFallen)
    {
        Debug.Log("Processing Shubhankar Ability - Runs: " + runsOnCurrentBall);

        if (wicketFallen == true)
        {
            OnWicketFallen(batsmanData, bowlerData);
        }
    }

    private void OnWicketFallen(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData )
    {
        bowlerData.BowlingPower = bowlerData.BowlingPower + 1;

        bowlerData.UpdatePlayerDataDuringMatch(bowlerData.Defense, bowlerData.BattingPower, bowlerData.BowlingPower);
        battleView.BowlingPowerGainedTextEffect(1.ToString());
        battleView.UpdateUIDuringBattle(playerLineupView,batsmanData,bowlerData);
    }
}