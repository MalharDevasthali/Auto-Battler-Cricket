using UnityEngine;

[System.Serializable]
public class PlayerDataDuringMatch
{
    public string playerName;
    public Sprite playerSprite;
    public int BattingPower;
    public int BowlingPower;
    public int Defense;
    public string SpecialAbility;
    public PlayerRole role;

    public PlayerDataDuringMatch() { }

    public PlayerDataDuringMatch(PlayerData src, int runtimeDefense)
    {
        playerName = src.playerName;
        playerSprite = src.playerSprite;
        BattingPower = src.BattingPower;
        BowlingPower = src.BowlingPower;
        Defense = runtimeDefense;
        SpecialAbility = src.SpecialAbility;
        role = src.role;
    }

    public void UpdatePlayerDataDuringMatch(int runtimeDefense, int runtimeBattingPower, int runtimeBowlingPower)
    {
        BattingPower = runtimeBattingPower;
        BowlingPower = runtimeBowlingPower;
        Defense = runtimeDefense;
    }
    public PlayerDataDuringMatch(PlayerData src, int runtimeDefense, int runtimeBattingPower, int runtimeBowlingPower)
    {
        playerName = src.playerName;
        playerSprite = src.playerSprite;
        BattingPower = runtimeBattingPower;
        BowlingPower = runtimeBowlingPower;
        Defense = runtimeDefense;
        SpecialAbility = src.SpecialAbility;
        role = src.role;
    }
}
