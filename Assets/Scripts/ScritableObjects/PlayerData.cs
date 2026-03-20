using UnityEngine;

public enum PlayerRole
{
    Batsman,
    Bowler
}

[CreateAssetMenu(fileName = "New Player", menuName = "ScriptableObject/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Basic Info")]
    public string playerName;
    public PlayerRole role;

    [Header("Stats")]
    public int power;
    public int skill;

    [Header("Meta")]
    public int rarity;

    [Header("Visuals")]
    public Sprite cardSprite;
}