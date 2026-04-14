using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityQueueSystem : MonoBehaviour
{
    public event Action<PlayerDataDuringMatch,PlayerDataDuringMatch, int,float> OnRunsScored;
    public event Action<PlayerDataDuringMatch,PlayerDataDuringMatch,float> OnWicketFallen;
    public event Action<PlayerDataDuringMatch,PlayerDataDuringMatch,float> OnComesToBat;



    public void Reset()
    {
        OnRunsScored = null;
        OnWicketFallen = null;
        OnComesToBat = null;
    }
}