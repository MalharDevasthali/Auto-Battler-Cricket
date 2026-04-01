using System;
using System.Collections.Generic;
using UnityEngine;

public  class GameService : MonoBehaviour
{
    public enum Innings
    {
        Batting,
        Bowling
    }

    private Innings currentInnings;
    private const int MaxBatsmanCount = 6;
    private static List<PlayerData> selectedTeam = new List<PlayerData>(MaxBatsmanCount);

    private void Start()
    {
        currentInnings = Innings.Batting;

        InitilizeTeamList();
    }

    private void InitilizeTeamList()
    {
        for (int i = 0; i < MaxBatsmanCount; i++)
        {
            selectedTeam.Add(null);
        }
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
        selectedTeam[playingOrder] = playerData;
    }

    public void RemovePlayerData(PlayerData playerData,int playingOrder )
    {
        selectedTeam[playingOrder] = null;
    }

    public List<PlayerData> GetSelectedTeam()
    {
        return selectedTeam;
    }

}
