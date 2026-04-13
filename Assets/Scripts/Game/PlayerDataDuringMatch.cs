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
    public PlayerAbility runTimeAbility;
    public int playerRunsDuringMatch;


    public void UpdatePlayerDataDuringMatch(int runtimeDefense, int runtimeBattingPower, int runtimeBowlingPower)
    {
        BattingPower = runtimeBattingPower;
        BowlingPower = runtimeBowlingPower;
        Defense = runtimeDefense;   
    }

    public void SetIndivisualRunsScored(int runs)
    {
        playerRunsDuringMatch += runs;
    }

    public PlayerDataDuringMatch(PlayerData playerData)
    {
        playerName = playerData.playerName;
        playerSprite = playerData.playerSprite;
        SpecialAbility = playerData.SpecialAbility;
        role = playerData.role;
        runTimeAbility = playerData.playerAbility;

        BattingPower = playerData.BattingPower;
        Defense = playerData.Defense;
        BowlingPower= playerData.BowlingPower;
    }

}
