using System;
using UnityEngine;


public enum PlayerSate
{
    Locked,
    Unlocked
}

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

[CreateAssetMenu(fileName = "New Player", menuName = "ScriptableObject/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Basic Info")]
    [HideInInspector]public int playerID;
    public string playerName;
    public PlayerSate playerSate;
    public PlayerRole role;
    public PlayerRarity rarity;


    [Header("Stats")]
    public int PlayerLevel;
    public int BattingPower;
    public int BowlingPower;
    public int Defense;
    public string specialAbilityLevel1;
    public string specialAbilityLevel2;
    public string specialAbilityLevel3;
    public PlayerAbility playerAbility;

    [Header("Visuals")]
    public Sprite playerSprite;
}