using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Vedant Ability")]
public class VedantAbility : PlayerAbility
{
    public override void OnRunsScored(PlayerDataDuringMatch player, int runs)
    {
        if (player != owner) return;

        if (runs == 4)
        {
            owner.Defense += 1;
        }
    }

    public override void OnWicketFallen(PlayerDataDuringMatch player) { }
}