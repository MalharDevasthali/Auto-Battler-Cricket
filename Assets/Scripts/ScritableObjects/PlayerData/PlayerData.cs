using System;
using UnityEngine;

public enum PlayerRole
{
    Batsman,
    Bowler
}
[Serializable]

[CreateAssetMenu(fileName = "New Player", menuName = "ScriptableObject/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Basic Info")]
    public string playerName;
    public PlayerRole role;

    [Header("Stats")]
    public int BattingPower;
    public int BowlingPower;
    public int Defense;
    public string SpecialAbility;
    public PlayerAbility playerAbility;

    [Header("Meta")]
    public int rarity;

    [Header("Visuals")]
    public Sprite playerSprite;
}