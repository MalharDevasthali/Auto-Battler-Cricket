using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventService : MonoBehaviour
{
    public event Action<PlayerDataDuringMatch,PlayerDataDuringMatch, int> OnRunsScored;
    public event Action<PlayerDataDuringMatch,PlayerDataDuringMatch> OnWicketFallen;
    public event Action<PlayerDataDuringMatch,PlayerDataDuringMatch> OnComesToBat;

    private Queue<IEnumerator> eventQueue = new Queue<IEnumerator>();
    private bool isProcessing = false;

    public void RaiseRunsScored(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData, int runs,float delay )
    {

         OnRunsScored?.Invoke(batsmanData,bowlerData, runs);
       // Enqueue(RunsScoredRoutine(batsmanData, runs, delay));
    }

    public void RaiseWicketFallen(PlayerDataDuringMatch batsmanData , PlayerDataDuringMatch bowlerData)
    {
        OnWicketFallen?.Invoke(batsmanData,bowlerData);
    }

    public void RaiseOnComesToBat(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData)
    {
        OnComesToBat?.Invoke(batsmanData,bowlerData);
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

    private IEnumerator RunsScoredRoutine(PlayerDataDuringMatch player, int runs , float delay)
    {
      //  OnRunsScored?.Invoke(player, runs);

        yield return new WaitForSeconds(delay);
    }


    public void Reset()
    {
        OnRunsScored = null;
        OnWicketFallen = null;
        OnComesToBat = null;
    }
}