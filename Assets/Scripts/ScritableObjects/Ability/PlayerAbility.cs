using UnityEngine;

public abstract class PlayerAbility : ScriptableObject
{
    public abstract void Init();
    public abstract void EventUnSubscribe();
}