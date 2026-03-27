using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.WSA;

public class TeamSelectionController : MonoBehaviour
{

    private int maxPlayers = 6;
    private int selectedPlayerCount = 0;
   

    [Header("UI")]
    [SerializeField] List<TeamCardView> TeamPlayerSlots = new List<TeamCardView>();
    [SerializeField] List<PlayerCardView> AllPlayers = new List<PlayerCardView>();
    [SerializeField] TextMeshProUGUI SelectedPlayersCounterText;
    [SerializeField] Button startBattleButton;

    private void Awake()
    {
        startBattleButton.onClick.AddListener(OnStartBattleButtonClick);
    }
    public bool CanSelect(PlayerRole role)
    {

       return selectedPlayerCount < maxPlayers;
    }

    public void AddPlayer(PlayerData player)
    {
        if (TeamPlayerSlots.Exists(t => t.data != null && t.data == player)) return;


        for (int i = 0; i < TeamPlayerSlots.Count; i++)
        {
            if (TeamPlayerSlots[i].data == null)
            {
                TeamPlayerSlots[i].AddToTeam(player, i + 1);
                break;
            }
        }
        selectedPlayerCount = TeamPlayerSlots.Count(t => t.data != null);
        Debug.Log("Total Team Players Are: " + selectedPlayerCount);
        SelectedPlayersCounterText.text = "("+selectedPlayerCount + "/6)";
    }

    public void DeselectPlayer(PlayerData data)
    {
        for (int i = 0; i < AllPlayers.Count; i++)
        {
            if (AllPlayers[i].data == data)
            {
                AllPlayers[i].Deselect();
                break;
            }
        }
    }
    public void RemovePlayer(PlayerData data)
    {
        for (int i = 0; i < TeamPlayerSlots.Count; i++)
        {
            if (TeamPlayerSlots[i].data == data)
            {
                TeamPlayerSlots[i].RemoveFromTeam();
                break;
            }
        }

        selectedPlayerCount = TeamPlayerSlots.Count(t => t.data != null);
        Debug.Log("Total Team Players Are: " + selectedPlayerCount);
        SelectedPlayersCounterText.text = "(" + selectedPlayerCount + "/6)";
    }

    private void OnStartBattleButtonClick()
    {
        if (selectedPlayerCount == 6)
            SceneManager.LoadScene(1);
            
    }

}