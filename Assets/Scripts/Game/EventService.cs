using System;
using UnityEngine;

public class EventService : MonoBehaviour
{
    public event Action<PlayerDataDuringMatch, int> OnRunsScored;
    public event Action<PlayerDataDuringMatch> OnWicketFallen;

    // Raise
    public void RaiseRunsScored(PlayerDataDuringMatch player, int runs)
    {
        OnRunsScored?.Invoke(player, runs);
    }

    public void RaiseWicketFallen(PlayerDataDuringMatch batsmanWhoGotOut)
    {
        OnWicketFallen?.Invoke(batsmanWhoGotOut);
    }

    public void Reset()
    {
        OnRunsScored = null;
        OnWicketFallen = null;
    }
}