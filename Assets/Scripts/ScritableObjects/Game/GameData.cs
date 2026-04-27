using System;
using System.Collections.Generic;
using UnityEngine;

public enum Innings
{
    Batting,
    Bowling
}

[Serializable]
[CreateAssetMenu(fileName = "Game Data", menuName = "ScriptableObject/Game Data")]
public class GameData : ScriptableObject
{
    public Innings currentInnings;
    public List<PlayerData> batsmenData = new List<PlayerData>();
    public PlayerData bowlerData;
    public int unlockedTeamSlots = 3;

}

