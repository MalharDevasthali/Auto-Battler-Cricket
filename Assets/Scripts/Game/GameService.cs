using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public  class GameService : MonoBehaviour
{

    [SerializeField] private GameData gameData;
    private void Start()
    {
        gameData.currentInnings = Innings.Batting;
    }

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

    public List<PlayerData> GetSelectedTeam()
    {
        return gameData.batsmenData.Where(playerData => playerData != null).ToList();
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
