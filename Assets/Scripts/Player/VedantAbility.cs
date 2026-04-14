using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Vedant Ability")]
public class VedantAbility : PlayerAbility
{

    public int defenceBoostAfterAbility = 1;
    public int battingPowerBoostAfterAbility = 1;

    private BattleView battleView;
    private PlayerLineupView playerLineupView;
    private AbilityQueueSystem abilityQueueSystem;

    private int lastProcessRuns = 0;

    public override void Init(BattleView battleView, PlayerLineupView playerLineupView, AbilityQueueSystem abilityQueueSystem)
    {
        this.battleView = battleView;
        this.playerLineupView = playerLineupView;
        this.abilityQueueSystem = abilityQueueSystem;
        Debug.Log("Vedant Ability Got Subscribed");
    }
  
    public override async Task ProcessAbility(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runsOnCurrentBall, bool wicketFallen)
    {
        if (lastProcessRuns < 4 && batsmanData.playerRunsDuringMatch >= 4)
        {
            await QueueAbilityAsync(() => FourRunsOrMore(batsmanData, bowlerData));
        }

        if (lastProcessRuns < 6 && batsmanData.playerRunsDuringMatch >= 6)
        {
            await QueueAbilityAsync(() => SixRunsOrMore(batsmanData, bowlerData));
        }

        lastProcessRuns = batsmanData.playerRunsDuringMatch;
    }

    private Task QueueAbilityAsync(System.Func<Task> abilityAction)
    {
        if (abilityQueueSystem == null)
        {
            throw new System.InvalidOperationException("AbilityQueueSystem is not assigned.");
        }

        return abilityQueueSystem.EnqueueAbility(abilityAction);
    }

    private Task FourRunsOrMore(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData)
    {
        Debug.Log("Vedant Defence Ability Got Triggered,Runs: " + batsmanData.playerRunsDuringMatch);
        batsmanData.Defense += defenceBoostAfterAbility;
        batsmanData.UpdatePlayerDataDuringMatch(batsmanData.Defense, batsmanData.BattingPower, batsmanData.BowlingPower);
        battleView.DefenseGainedTextEffect(defenceBoostAfterAbility.ToString());

        playerLineupView.UpdatePlayerView(batsmanData.Defense,batsmanData.BattingPower);
        battleView.UpdateUIDuringBattle(playerLineupView, batsmanData, bowlerData);
        return Task.CompletedTask;
    }

    private Task SixRunsOrMore(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData)
    {
        Debug.Log("Vedant Batting Power Ability Got Triggered,Runs: " + batsmanData.playerRunsDuringMatch);
        batsmanData.BattingPower += battingPowerBoostAfterAbility;
        batsmanData.UpdatePlayerDataDuringMatch(batsmanData.Defense, batsmanData.BattingPower, batsmanData.BowlingPower);
        battleView.BattingPowerGainedTextEffect(battingPowerBoostAfterAbility.ToString());

        playerLineupView.UpdatePlayerView(batsmanData.Defense,batsmanData.BattingPower);
        battleView.UpdateUIDuringBattle(playerLineupView, batsmanData, bowlerData);
        return Task.CompletedTask;
    }
}
