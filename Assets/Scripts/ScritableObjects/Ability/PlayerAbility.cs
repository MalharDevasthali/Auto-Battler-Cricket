using System.Threading.Tasks;
using UnityEngine;

public abstract class PlayerAbility : ScriptableObject
{
    public abstract void Init(BattleView battleView, PlayerLineupView playerLineupView,AbilityQueueSystem abilityQueueSystem);
 
    public abstract Task ProcessAbility(PlayerDataDuringMatch batsmanData, PlayerDataDuringMatch bowlerData , int runsOnCurrentBall , bool wicketFallen);


    public virtual void SetCurrentBatsmanView(PlayerLineupView currentBatsmanView)
    {
        //This method is required for only bowlers, because bowlers need to set currentBatsmanView after wicket is fallen
    }
}
