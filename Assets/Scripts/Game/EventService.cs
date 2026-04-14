using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;

public class EventService : MonoBehaviour
{
    public event Action<PlayerDataDuringMatch,PlayerDataDuringMatch, int,float> OnRunsScored;
    public event Action<PlayerDataDuringMatch,PlayerDataDuringMatch,float> OnWicketFallen;
    public event Action<PlayerDataDuringMatch,PlayerDataDuringMatch,float> OnComesToBat;

    private Queue<IEnumerator> eventQueue = new Queue<IEnumerator>();
    private bool isProcessing = false;

    public void RaiseRunsScored(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runs,float abilityDelay)
    {

        // OnRunsScored?.Invoke(batsmanData,bowlerData, runs, abilityDelay);
        Enqueue(RunsScoredRoutine(batsmanData, bowlerData, runs, abilityDelay));
    }

    public void RaiseWicketFallen(PlayerDataDuringMatch batsmanData , PlayerDataDuringMatch bowlerData,float abilityDelay)
    {
        // TBD
       // OnWicketFallen?.Invoke(batsmanData,bowlerData, abilityDelay);
        //Enqueue(RunsScoredRoutine(batsmanData, bowlerData, abilityDelay));
    }

    public void RaiseOnComesToBat(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, float abilityDelay)
    {
        OnComesToBat?.Invoke(batsmanData,bowlerData,abilityDelay);
    }

    private void Enqueue(IEnumerator routine)
    {
        eventQueue.Enqueue(routine);

        if (!isProcessing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isProcessing = true;

        while (eventQueue.Count > 0)
        {
            yield return eventQueue.Dequeue(); //sequential execution
        }

        isProcessing = false;
    }

    private IEnumerator RunsScoredRoutine(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runs , float delay)
    {
        OnRunsScored?.Invoke(batsmanData,bowlerData, runs, delay);
        yield return new WaitForSeconds(delay);
    }


    public void Reset()
    {
        OnRunsScored = null;
        OnWicketFallen = null;
        OnComesToBat = null;
    }
}