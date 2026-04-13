using UnityEngine;

public abstract class PlayerAbility : ScriptableObject
{
    public abstract void Init(BattleView battleView, PlayerLineupView playerLineupView);
    public abstract void EventUnSubscribe();
}