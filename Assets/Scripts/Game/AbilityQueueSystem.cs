using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AbilityQueueSystem : MonoBehaviour
{
    [SerializeField] private float delayBetweenAbilities = 2f;

    private readonly Queue<QueuedAbility> queuedAbilities = new Queue<QueuedAbility>();
    private bool isProcessing;
    private TaskCompletionSource<bool> queueDrainCompletionSource = CreateCompletedSource();

    public Task EnqueueAbility(Func<Task> abilityAction)
    {
        if (abilityAction == null)
        {
            return Task.CompletedTask;
        }

        var queuedAbility = new QueuedAbility(abilityAction);
        queuedAbilities.Enqueue(queuedAbility);

        if (!isProcessing)
        {
            queueDrainCompletionSource = new TaskCompletionSource<bool>();
            _ = ProcessQueueAsync();
        }

        return queuedAbility.Completion.Task;
    }

    public Task WaitForAllAbilitiesAsync()
    {
        return isProcessing || queuedAbilities.Count > 0
            ? queueDrainCompletionSource.Task
            : Task.CompletedTask;
    }

    private async Task ProcessQueueAsync()
    {
        isProcessing = true;

        try
        {
            while (queuedAbilities.Count > 0)
            {
                QueuedAbility queuedAbility = queuedAbilities.Dequeue();

                await Task.Delay(TimeSpan.FromSeconds(delayBetweenAbilities));
                await queuedAbility.ExecuteAsync();
            }
        }
        finally
        {
            isProcessing = false;
            queueDrainCompletionSource.TrySetResult(true);
        }
    }

    private static TaskCompletionSource<bool> CreateCompletedSource()
    {
        var completionSource = new TaskCompletionSource<bool>();
        completionSource.SetResult(true);
        return completionSource;
    }

    private sealed class QueuedAbility
    {
        private readonly Func<Task> abilityAction;

        public QueuedAbility(Func<Task> abilityAction)
        {
            this.abilityAction = abilityAction;
            Completion = new TaskCompletionSource<bool>();
        }

        public TaskCompletionSource<bool> Completion { get; }

        public async Task ExecuteAsync()
        {
            try
            {
                await abilityAction();
                Completion.TrySetResult(true);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                Completion.TrySetException(exception);
            }
        }
    }
}
