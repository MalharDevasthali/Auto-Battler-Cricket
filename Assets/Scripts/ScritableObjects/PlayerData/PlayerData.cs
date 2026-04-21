using System;
using UnityEngine;

public enum PlayerRole
{
    Batsman,
    Bowler
}

public enum PlayerRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
[Serializable]

[CreateAssetMenu(fileName = "New Player", menuName = "ScriptableObject/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Basic Info")]
    [HideInInspector]public int playerID;
    public string playerName;
    public PlayerRole role;
    public PlayerRarity rarity;


    [Header("Stats")]
    public int BattingPower;
    public int BowlingPower;
    public int Defense;
    public string SpecialAbility;
    public PlayerAbility playerAbility;

    [Header("Visuals")]
    public Sprite playerSprite;
}