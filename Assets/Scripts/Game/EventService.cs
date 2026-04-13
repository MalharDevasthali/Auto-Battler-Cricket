using System;
using UnityEngine;

public class EventService : MonoBehaviour
{
    public event Action<PlayerDataDuringMatch, int> OnRunsScored;
    public event Action<PlayerDataDuringMatch> OnWicketFallen;
    public event Action<PlayerDataDuringMatch> OnComesToBat;


    public void RaiseRunsScored(PlayerDataDuringMatch player, int runs)
    {
        OnRunsScored?.Invoke(player, runs);
    }

    public void RaiseWicketFallen(PlayerDataDuringMatch batsmanWhoGotOut)
    {
        OnWicketFallen?.Invoke(batsmanWhoGotOut);
    }

    public void RaiseOnComesToBat(PlayerDataDuringMatch batsmanWhoCameTobat)
    {
        OnComesToBat?.Invoke(batsmanWhoCameTobat);
    }

    public void Reset()
    {
        OnRunsScored = null;
        OnWicketFallen = null;
        OnComesToBat = null;
    }
}