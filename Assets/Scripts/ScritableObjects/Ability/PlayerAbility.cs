using UnityEngine;

public abstract class PlayerAbility : ScriptableObject
{
    public abstract void OnRunsScored(PlayerDataDuringMatch playerRuntimeData, int runs);
    public abstract void OnWicketTaken(PlayerDataDuringMatch playerRuntimeData);
}
