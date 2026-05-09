using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public  class GameService : MonoBehaviour
{

    [SerializeField] private GameData gameData;
    [SerializeField] private LeaugeData currentLeauge;
  
    public Innings GetCurrentInnings()
    { 
        return gameData.currentInnings; 
    }
    public void SetCurrentInnings(Innings currentInnings)
    {
        gameData.currentInnings = currentInnings;
    }
    public void AddBatsman(PlayerData batsmanData,int playingOrder)
    {
        EnsureTeamSlotExists(playingOrder);
        gameData.batsmenData[playingOrder] = batsmanData;
    }
    public void AddBowler(PlayerData bowlerData)
    {
        gameData.bowlerData = bowlerData;
    }

    public void RemoveBatsman(PlayerData batsmanData,int playingOrder )
    {
        if (playingOrder < 0 || playingOrder >= gameData.batsmenData.Count) return;

        gameData.batsmenData[playingOrder] = null;
        TrimEmptySlotsFromEnd();
    }

    public void ClearSelectedTeam()
    {
        gameData.batsmenData.Clear();
    }

    public List<PlayerData> GetPlayerBatmanTeam()
    {
        return gameData.batsmenData.Where(playerData => playerData != null).ToList();
    }
    public PlayerData GetPlayerBowler()
    {
        return gameData.bowlerData;
    }

    public List<PlayerData> GetCPUBatsmanTeam(int matchNumber)
    {
        return currentLeauge.groupMatches[matchNumber].batsmanTeam;
    }
    public PlayerData GetCPUBowler(int matchNumber)
    {
        return currentLeauge.groupMatches[matchNumber].bowler;
    }

    public LeaugeData GetLeaugeData()
    {
        return currentLeauge;
    }
    public GameData GetGameData()
    {
        return gameData;
    }

    public int GetUnlockedSlots()
    {
        return gameData.unlockedTeamSlots;
    }

    private void EnsureTeamSlotExists(int playingOrder)
    {
        while (gameData.batsmenData.Count <= playingOrder)
        {
            gameData.batsmenData.Add(null);
        }
    }

    private void TrimEmptySlotsFromEnd()
    {
        for (int i = gameData.batsmenData.Count - 1; i >= 0; i--)
        {
            if (gameData.batsmenData[i] != null) break;

            gameData.batsmenData.RemoveAt(i);
        }
    }

}
