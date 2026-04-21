using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RandomTeamGenerator : MonoBehaviour
{
    private const int RandomTeamSize = 3;
    private int selectedPlayerCount = 0;
   

    [Header("UI")]
    [SerializeField] private List<TeamCardView> TeamPlayerSlots = new List<TeamCardView>();
    [SerializeField] private List<PlayerCardView> AllPlayers = new List<PlayerCardView>();
    [SerializeField] private Button startBattleButton;

    [Header("Sounds")]
    [SerializeField] private AudioClip buttonClickSound;


    private void Awake()
    {
        if (startBattleButton != null)
            startBattleButton.onClick.AddListener(OnStartBattleButtonClick);
    }

    private IEnumerator Start()
    {
        yield return null;
        SelectRandomTeam();
    }

    private void SelectRandomTeam()
    {
        ClearTeamSlots();

        List<TeamCardView> availableTeamSlots = TeamPlayerSlots
            .Where(teamPlayerSlot => teamPlayerSlot != null)
            .Take(RandomTeamSize)
            .ToList();

        if (availableTeamSlots.Count < RandomTeamSize)
        {
            Debug.LogError("Not enough team slots to generate a random team.");
            return;
        }

        List<PlayerData> unlockedPlayers = AllPlayers
            .Where(playerCard => playerCard != null && playerCard.data != null)
            .Select(playerCard => playerCard.data)
            .Distinct()
            .ToList();

        if (unlockedPlayers.Count < RandomTeamSize)
        {
            Debug.LogError("Not enough unlocked players to generate a random team.");
            return;
        }

        for (int i = 0; i < RandomTeamSize; i++)
        {
            int randomIndex = Random.Range(0, unlockedPlayers.Count);
            PlayerData randomPlayer = unlockedPlayers[randomIndex];
            unlockedPlayers.RemoveAt(randomIndex);

            availableTeamSlots[i].AddToTeam(randomPlayer, i);
        }
    }

    private void ClearTeamSlots()
    {
        foreach (TeamCardView teamPlayerSlot in TeamPlayerSlots)
        {
            if (teamPlayerSlot != null && teamPlayerSlot.data != null)
            {
                teamPlayerSlot.RemoveFromTeam();
            }
        }

        selectedPlayerCount = 0;
    }


    private void OnStartBattleButtonClick()
    {
        ServiceLocator.Instance.SoundService.PlaySound(buttonClickSound);
        if (selectedPlayerCount == RandomTeamSize)
            SceneManager.LoadScene(1);
            
    }

}
