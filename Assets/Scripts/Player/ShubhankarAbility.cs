using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shubhankar Ability")]
public class ShubhankarAbility : PlayerAbility
{
    private AbilityQueueSystem abilityQueueSystem;

    private BattleView battleView;
    private PlayerLineupView playerLineupView;


    public override void Init(BattleView battleView, PlayerLineupView playerLineupView,AbilityQueueSystem abilityQueueSystem)
    {
     
        this.battleView = battleView;
        this.abilityQueueSystem = abilityQueueSystem;
     
    }
 
    public override void SetCurrentBatsmanView(PlayerLineupView currentBatsmanView)
    {
        playerLineupView = currentBatsmanView;
    }

    public override async Task ProcessAbility(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runsOnCurrentBall, bool wicketFallen)
    {

        if(wicketFallen == true)
        {
            Debug.Log(" Shubhankar Ability Triggered");
            await QueueAbilityAsync(() => IncreaseBowlingPower(batsmanData, bowlerData));
            await QueueAbilityAsync(() => ReduceBattingPower(batsmanData, bowlerData));
        }

    }

    private Task QueueAbilityAsync(System.Func<Task> abilityAction)
    {
        if (abilityQueueSystem == null)
        {
            throw new System.InvalidOperationException("AbilityQueueSystem is not assigned.");
        }

        return abilityQueueSystem.EnqueueAbility(abilityAction);
    }


    private Task IncreaseBowlingPower(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData )
    {
        bowlerData.BowlingPower = bowlerData.BowlingPower + 1;

        bowlerData.UpdatePlayerDataDuringMatch(bowlerData.Defense, bowlerData.BattingPower, bowlerData.BowlingPower);
        batsmanData.UpdatePlayerDataDuringMatch(batsmanData.Defense,batsmanData.BattingPower, batsmanData.BowlingPower);

        battleView.BowlingPowerGainedTextEffect(1.ToString());
        battleView.UpdateUIDuringBattle(playerLineupView,batsmanData,bowlerData);

        return Task.CompletedTask;
    }

    private Task ReduceBattingPower(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData)
    {
        batsmanData.BattingPower = batsmanData.BattingPower - 1;
        
        bowlerData.UpdatePlayerDataDuringMatch(bowlerData.Defense, bowlerData.BattingPower, bowlerData.BowlingPower);
        batsmanData.UpdatePlayerDataDuringMatch(batsmanData.Defense, batsmanData.BattingPower, batsmanData.BowlingPower);
       
        battleView.BattingPowerReducedTextEffect(1.ToString());
        battleView.UpdateUIDuringBattle(playerLineupView, batsmanData, bowlerData);

        return Task.CompletedTask;
    }
}
