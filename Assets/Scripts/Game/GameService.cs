using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public  class GameService : MonoBehaviour
{
    public enum Innings
    {
        Batting,
        Bowling
    }

    private Innings currentInnings;
    private static List<PlayerData> selectedTeam = new List<PlayerData>();

    private void Start()
    {
        currentInnings = Innings.Batting;
    }

    public Innings GetCurrentInnings()
    { 
        return currentInnings; 
    }
    public void SetCurrentInnings(Innings currentInnings)
    {
        this.currentInnings = currentInnings;
    }
    public void AddPlayerData(PlayerData playerData,int playingOrder)
    {
        EnsureTeamSlotExists(playingOrder);
        selectedTeam[playingOrder] = playerData;
    }

    public void RemovePlayerData(PlayerData playerData,int playingOrder )
    {
        if (playingOrder < 0 || playingOrder >= selectedTeam.Count) return;

        selectedTeam[playingOrder] = null;
        TrimEmptySlotsFromEnd();
    }

    public void ClearSelectedTeam()
    {
        selectedTeam.Clear();
    }

    public List<PlayerData> GetSelectedTeam()
    {
        return selectedTeam.Where(playerData => playerData != null).ToList();
    }

    private void EnsureTeamSlotExists(int playingOrder)
    {
        while (selectedTeam.Count <= playingOrder)
        {
            selectedTeam.Add(null);
        }
    }

    private void TrimEmptySlotsFromEnd()
    {
        for (int i = selectedTeam.Count - 1; i >= 0; i--)
        {
            if (selectedTeam[i] != null) break;

            selectedTeam.RemoveAt(i);
        }
    }

}
