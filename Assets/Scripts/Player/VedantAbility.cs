using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Vedant Ability")]
public class VedantAbility : PlayerAbility
{
    public override void OnRunsScored(PlayerDataDuringMatch player, int runs)
    {
        if (runs == 4)
        {
            player.Defense += 1;
        }
    }

    public override void OnWicketTaken(PlayerDataDuringMatch player) { }
}