using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Vedant Ability")]
public class VedantAbility : PlayerAbility
{

    public int defenceBoostAfterAbility = 1;
    public int battingPowerBoostAfterAbility = 1;

    private EventService eventService;

    private BattleView battleView;
    private PlayerLineupView playerLineupView;

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

    private async void OnRunsScored(PlayerDataDuringMatch player, int runs)
    {
        Debug.Log("Vedant Runs: " + runs);
        if ( runs >= 4)
        {
            await Task.Delay(2000);
            Debug.Log("Vedant Defence Ability Got Triggered,Runs: "+runs);
            player.Defense += defenceBoostAfterAbility;
            player.UpdatePlayerDataDuringMatch(player.Defense, player.BattingPower, player.BowlingPower);
            battleView.DefenseGainedTextEffect(defenceBoostAfterAbility.ToString());

            playerLineupView.UpdateDefense(player.Defense);
            battleView.UpdateCurrentBatsman(player);
        }

        if (runs >= 6)
        {
            await Task.Delay(2000);
            Debug.Log("Vedant Batting Power Ability Got Triggered,Runs: " + runs);
            player.BattingPower += battingPowerBoostAfterAbility;
            player.UpdatePlayerDataDuringMatch(player.Defense, player.BattingPower, player.BowlingPower);
            battleView.BattingPowerGainedTextEffect(battingPowerBoostAfterAbility.ToString());

            playerLineupView.UpdateBattingPower(player.BattingPower);
            battleView.UpdateCurrentBatsman(player);
        }
        await Task.Delay(0);
    }
}