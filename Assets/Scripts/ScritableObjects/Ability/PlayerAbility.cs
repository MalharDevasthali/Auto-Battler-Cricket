using UnityEngine;

public abstract class PlayerAbility : ScriptableObject
{
    public abstract void Init(BattleView battleView, PlayerLineupView playerLineupView);
    public abstract void ProcessAbility(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData , int runsOnCurrentBall);
    public abstract void EventUnSubscribe();
}