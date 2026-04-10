using UnityEngine;

public abstract class PlayerAbility : ScriptableObject
{

    protected PlayerDataDuringMatch owner;
    protected EventService eventService;

    public void Init(PlayerDataDuringMatch player)
    {
        owner = player;
        eventService = ServiceLocator.Instance.EventService;

        Subscribe();
    }

    public void Dispose()
    {
        Unsubscribe();
    }

    protected virtual void Subscribe()
    {
        eventService.OnRunsScored += OnRunsScored;
        eventService.OnWicketFallen += OnWicketFallen;
    }

    protected virtual void Unsubscribe()
    {
        eventService.OnRunsScored -= OnRunsScored;
        eventService.OnWicketFallen -= OnWicketFallen;
    }


    public abstract void OnRunsScored(PlayerDataDuringMatch playerRuntimeData, int runs);
    public abstract void OnWicketFallen(PlayerDataDuringMatch playerRuntimeData);
}
