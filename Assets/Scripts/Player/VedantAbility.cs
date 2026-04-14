using System.Threading.Tasks;
using UnityEngine;
using static Unity.Collections.Unicode;

[CreateAssetMenu(menuName = "Abilities/Vedant Ability")]
public class VedantAbility : PlayerAbility
{

    public int defenceBoostAfterAbility = 1;
    public int battingPowerBoostAfterAbility = 1;

    private BattleView battleView;
    private PlayerLineupView playerLineupView;

    private int lastProcessRuns = 0;

    public override void Init(BattleView battleView, PlayerLineupView playerLineupView)
    {
     
        this.battleView = battleView;
        this.playerLineupView = playerLineupView;
        Debug.Log("Vedant Ability Got Subscribed");
    }
  
    public override void ProcessAbility(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runsOnCurrentBall, bool wicketFallen)
    {
      
        if (lastProcessRuns < 4 && batsmanData.playerRunsDuringMatch >= 4)
        {
            FourRunsOrMore(batsmanData,bowlerData,runsOnCurrentBall);
        }

        if (lastProcessRuns < 6 && batsmanData.playerRunsDuringMatch >= 6)
        {
            SixRunsOrMore(batsmanData, bowlerData, runsOnCurrentBall);
        }
        lastProcessRuns = runsOnCurrentBall;
    }

    private void  FourRunsOrMore(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runsOnCurrentBall)
    {
        Debug.Log("Vedant Defence Ability Got Triggered,Runs: " + batsmanData.playerRunsDuringMatch);
        batsmanData.Defense += defenceBoostAfterAbility;
        batsmanData.UpdatePlayerDataDuringMatch(batsmanData.Defense, batsmanData.BattingPower, batsmanData.BowlingPower);
        battleView.DefenseGainedTextEffect(defenceBoostAfterAbility.ToString());

        playerLineupView.UpdateDefense(batsmanData.Defense);
        battleView.UpdateUIDuringBattle(playerLineupView, batsmanData, bowlerData);

    }

    private void SixRunsOrMore(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runsOnCurrentBall)
    {
        Debug.Log("Vedant Batting Power Ability Got Triggered,Runs: " + batsmanData.playerRunsDuringMatch);
        batsmanData.BattingPower += battingPowerBoostAfterAbility;
        batsmanData.UpdatePlayerDataDuringMatch(batsmanData.Defense, batsmanData.BattingPower, batsmanData.BowlingPower);
        battleView.BattingPowerGainedTextEffect(battingPowerBoostAfterAbility.ToString());

        playerLineupView.UpdateBattingPower(batsmanData.BattingPower);
        battleView.UpdateUIDuringBattle(playerLineupView, batsmanData, bowlerData);
    }
}