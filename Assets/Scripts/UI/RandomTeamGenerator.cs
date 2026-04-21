using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.WSA;

public class RandomTeamGenerator : MonoBehaviour
{
    private int selectedPlayerCount = 0;
   

    [Header("UI")]
    [SerializeField] List<TeamCardView> TeamPlayerSlots = new List<TeamCardView>();
    [SerializeField] List<PlayerCardView> AllPlayers = new List<PlayerCardView>();
    [SerializeField] TextMeshProUGUI SelectedPlayersCounterText;
    [SerializeField] Button startBattleButton;

    [Header("Sounds")]
    [SerializeField] private AudioClip buttonClickSound;


    private void Awake()
    {
        startBattleButton.onClick.AddListener(OnStartBattleButtonClick);
    }

    public void AddPlayer(PlayerData player, int slotIndex)
    {
        if (TeamPlayerSlots.Exists(t => t.data == player)) return;

        if (TeamPlayerSlots[slotIndex].data != null) return; // optional safety

        TeamPlayerSlots[slotIndex].AddToTeam(player, slotIndex);

        selectedPlayerCount = TeamPlayerSlots.Count(t => t.data != null);
        Debug.Log("Total Team Players Are: " + selectedPlayerCount);
        SelectedPlayersCounterText.text = "(" + selectedPlayerCount + "/6)";
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

    public void SwapPlayers(int fromIndex, int toIndex)
    {
        var fromData = TeamPlayerSlots[fromIndex].data;
        var toData = TeamPlayerSlots[toIndex].data;
        TeamPlayerSlots[fromIndex].RemoveFromTeam();
        TeamPlayerSlots[toIndex].RemoveFromTeam();
        if (fromData != null)
            TeamPlayerSlots[toIndex].AddToTeam(fromData, toIndex);
        if (toData != null)
            TeamPlayerSlots[fromIndex].AddToTeam(toData, fromIndex);
    }

    private void OnStartBattleButtonClick()
    {
        ServiceLocator.Instance.SoundService.PlaySound(buttonClickSound);
        if (selectedPlayerCount == 6)
            SceneManager.LoadScene(1);
            
    }

}