using System.Collections.Generic;
using UnityEngine;

public class TeamSelectionService : MonoBehaviour
{
    [Header("Selection Settings")]
    public int maxPlayers = 6;

 
    public PlayerRole currentSelectionRole;

    private List<PlayerCard> selectedPlayers = new List<PlayerCard>();


    public bool CanSelect(PlayerRole role)
    {
        if (role != currentSelectionRole)
            return false;

        return selectedPlayers.Count < maxPlayers;
    }


    public void AddPlayer(PlayerCard player)
    {
        if (!selectedPlayers.Contains(player))
        {
            selectedPlayers.Add(player);
            Debug.Log("Selected Players: " + selectedPlayers.Count);
        }
    }


    public void RemovePlayer(PlayerCard player)
    {
        if (selectedPlayers.Contains(player))
        {
            selectedPlayers.Remove(player);
            Debug.Log("Selected Players: " + selectedPlayers.Count);
        }
    }


    public List<PlayerCard> GetSelectedPlayers()
    {
        return selectedPlayers;
    }


    public void SetSelectionRole(PlayerRole role)
    {
        currentSelectionRole = role;
        ClearSelection();
    }

    public void ClearSelection()
    {
        foreach (var player in selectedPlayers)
        {
            player.Deselect();
        }

        selectedPlayers.Clear();
    }
}