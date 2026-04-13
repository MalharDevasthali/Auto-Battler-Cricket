using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Vedant Ability")]
public class VedantAbility : PlayerAbility
{
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
        if ( runs % 4 == 0)
        {
            await Task.Delay(2000);
            Debug.Log("Vedant Ability Got Triggered,Runs: "+runs);
            player.Defense += 1;
            player.UpdatePlayerDataDuringMatch(player.Defense, player.BattingPower, player.BowlingPower);
            battleView.DefenseGainedTextEffect(1.ToString());
           // await Task.Delay(500);
            playerLineupView.UpdateDefense(player.Defense);
            battleView.UpdateCurrentBatsman(player);
        }
         await Task.Delay(0);
    }
}