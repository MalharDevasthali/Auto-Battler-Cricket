using System.Threading.Tasks;
using UnityEngine;

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

    private async void OnRunsScored(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runs)
    {
        Debug.Log("Vedant Runs: " + runs);
        if (lastProcessRuns < 4 && runs >= 4)
        {
            await Task.Delay(2000);
            Debug.Log("Vedant Defence Ability Got Triggered,Runs: "+runs);
            batsmanData.Defense += defenceBoostAfterAbility;
            batsmanData.UpdatePlayerDataDuringMatch(batsmanData.Defense, batsmanData.BattingPower, batsmanData.BowlingPower);
            battleView.DefenseGainedTextEffect(defenceBoostAfterAbility.ToString());

            playerLineupView.UpdateDefense(batsmanData.Defense);
            battleView.UpdateUIDuringBattle(playerLineupView, batsmanData,bowlerData);
        }

        if (lastProcessRuns < 6 && runs >= 6)
        {
            await Task.Delay(2000);
            Debug.Log("Vedant Batting Power Ability Got Triggered,Runs: " + runs);
            batsmanData.BattingPower += battingPowerBoostAfterAbility;
            batsmanData.UpdatePlayerDataDuringMatch(batsmanData.Defense, batsmanData.BattingPower, batsmanData.BowlingPower);
            battleView.BattingPowerGainedTextEffect(battingPowerBoostAfterAbility.ToString());

            playerLineupView.UpdateBattingPower(batsmanData.BattingPower);
            battleView.UpdateUIDuringBattle(playerLineupView, batsmanData,bowlerData);
        }
        lastProcessRuns = runs;
        await Task.Delay(0);
    }
}