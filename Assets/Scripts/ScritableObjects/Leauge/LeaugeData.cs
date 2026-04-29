using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Leauge", menuName = "ScriptableObject/Leauge Data")]
public class LeaugeData : ScriptableObject
{
    public List<MatchData> groupMatches;
    public MatchData semiFinal;
    public MatchData final;
}
