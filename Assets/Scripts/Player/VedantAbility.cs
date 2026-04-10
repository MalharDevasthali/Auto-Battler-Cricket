using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Vedant Ability")]
public class VedantAbility : PlayerAbility
{
    private EventService eventService;

    public override void Init()
    {
        eventService = ServiceLocator.Instance.EventService;

        eventService.OnRunsScored += OnRunsScored;
        Debug.Log("Vedant Ability Got Subscribed");
    }
    public override void EventUnSubscribe()
    {
        eventService.OnRunsScored -= OnRunsScored;
        Debug.Log("Vedant Ability Got Unsubscribed");
    }

    private void OnRunsScored(PlayerDataDuringMatch player, int runs)
    {
        Debug.Log("Vedant Runs: " + runs);
        if (runs == 4)
        {
            Debug.Log("Vedant Ability Got Triggered,Runs: "+runs);
            player.Defense += 1;
            player.UpdatePlayerDataDuringMatch(player.Defense, player.BattingPower, player.BowlingPower);
        }
    }
}