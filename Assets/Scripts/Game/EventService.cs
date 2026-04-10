using System;
using UnityEngine;

public class EventService : MonoBehaviour
{
    public event Action<PlayerDataDuringMatch, int> OnRunsScored;
    public event Action<PlayerDataDuringMatch> OnWicketFallen;

    public void RaiseRunsScored(PlayerDataDuringMatch player, int runs)
    {
        OnRunsScored?.Invoke(player, runs);
    }

    public void RaiseWicketFallen(PlayerDataDuringMatch player)
    {
        OnWicketFallen?.Invoke(player);
    }

    public void Reset()
    {
        OnRunsScored = null;
        OnWicketFallen = null;
    }
}