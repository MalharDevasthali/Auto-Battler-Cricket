using System.Collections.Generic;
using UnityEngine;

public class GameService : MonoBehaviour
{
    public enum Innings
    {
        Batting,
        Bowling
    }

    private Innings currentInnings;

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

    private List<PlayerData> selectedTeam = new List<PlayerData>();
    [SerializeField] private int TeamSize = 6;

    public void AddPlayerData(PlayerData playerData)
    {
        selectedTeam.Add(playerData);        
    }

    public void RemovePlayerData(PlayerData playerData)
    {
        selectedTeam.Remove(playerData);
    }

}
