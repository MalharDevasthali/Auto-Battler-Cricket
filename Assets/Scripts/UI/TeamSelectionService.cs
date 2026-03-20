using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamSelectionService : MonoBehaviour
{

    private int maxPlayers = 6;
    private int selectedPlayerCount = 0;
    [SerializeField] List<TeamCardView> team = new List<TeamCardView>();

    public bool CanSelect(PlayerRole role)
    {

       return selectedPlayerCount < maxPlayers;
    }

    public void AddPlayer(PlayerData player)
    {
        if (team.Exists(t => t.data != null && t.data == player)) return;


        for (int i = 0; i < team.Count; i++)
        {
            if (team[i].data == null)
            {
                team[i].AddToTeam(player, i + 1);
                break;
            }
        }
        selectedPlayerCount = team.Count(t => t.data != null);
        Debug.Log("Total Team Players Are: " + selectedPlayerCount);
    }


    public void RemovePlayer(PlayerData player)
    {
        for (int i = 0; i < team.Count; i++)
        {
            if (team[i].data == player)
            {
                team[i].RemoveFromTeam(); // you implement this
                break;
            }
        }

        selectedPlayerCount = team.Count(t => t.data != null);
        Debug.Log("Total Team Players Are: " + selectedPlayerCount);
    }

}