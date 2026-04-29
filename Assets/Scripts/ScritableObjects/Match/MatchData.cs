using System.Collections.Generic;
using UnityEngine;



public enum MatchStage
{
    Group,
    SemiFinal,
    Final
}

[CreateAssetMenu(fileName = "New Match", menuName = "ScriptableObject/Match Data")]

public class MatchData : ScriptableObject
{
    [Header("Meta")]
    public MatchStage stage;

    [Header("Opponent Team")]
    public List<PlayerData> batsmen;  
    public PlayerData bowler;

}
