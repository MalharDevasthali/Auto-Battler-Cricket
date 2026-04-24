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
    public void AddPlayerData(PlayerData playerData,int playingOrder)
    {
        EnsureTeamSlotExists(playingOrder);
        gameData.selectedTeam[playingOrder] = playerData;
    }

    public void RemovePlayerData(PlayerData playerData,int playingOrder )
    {
        if (playingOrder < 0 || playingOrder >= gameData.selectedTeam.Count) return;

        gameData.selectedTeam[playingOrder] = null;
        TrimEmptySlotsFromEnd();
    }

    public void ClearSelectedTeam()
    {
        gameData.selectedTeam.Clear();
    }

    public List<PlayerData> GetSelectedTeam()
    {
        return gameData.selectedTeam.Where(playerData => playerData != null).ToList();
    }
    public int GetUnlockedSlots()
    {
        return gameData.unlockedTeamSlots;
    }

    private void EnsureTeamSlotExists(int playingOrder)
    {
        while (gameData.selectedTeam.Count <= playingOrder)
        {
            gameData.selectedTeam.Add(null);
        }
    }

    private void TrimEmptySlotsFromEnd()
    {
        for (int i = gameData.selectedTeam.Count - 1; i >= 0; i--)
        {
            if (gameData.selectedTeam[i] != null) break;

            gameData.selectedTeam.RemoveAt(i);
        }
    }

}
