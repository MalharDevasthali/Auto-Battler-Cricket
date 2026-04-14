using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Gunde Ability")]
public class GundeAbility : PlayerAbility
{
 
    private BattleView battleView;
    private PlayerLineupView playerLineupView;
    private AbilityQueueSystem abilityQueueSystem;


    public override void Init(BattleView battleView, PlayerLineupView playerLineupView, AbilityQueueSystem abilityQueueSystem)
    {

        this.battleView = battleView;
        this.playerLineupView = playerLineupView;
        this.abilityQueueSystem = abilityQueueSystem;
    }

    public override async Task ProcessAbility(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runsOnCurrentBall, bool wicketFallen)
    {
    

        if(batsmanData.playerName == "Fat Ass Gunde")
        {
            Debug.Log(" Gunde Ability Triggered");
            await QueueAbilityAsync(() => OnComesToBat(batsmanData, bowlerData));
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


    private Task OnComesToBat(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData)
    {
        bowlerData.BowlingPower = bowlerData.BowlingPower - 1;

        bowlerData.UpdatePlayerDataDuringMatch(bowlerData.Defense, bowlerData.BattingPower, bowlerData.BowlingPower);
        battleView.BowlingPowerReducedTextEffect(1.ToString());
        battleView.UpdateUIDuringBattle(playerLineupView, batsmanData, bowlerData);
        return Task.CompletedTask;
    }

}
