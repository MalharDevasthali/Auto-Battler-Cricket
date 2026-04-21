using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RandomTeamGenerator : MonoBehaviour
{
    private const int RandomTeamSize = 3;

    [Header("UI")]
    [SerializeField] private List<TeamCardView> TeamPlayerSlots = new List<TeamCardView>();
    [SerializeField] private List<PlayerCardView> AllPlayers = new List<PlayerCardView>();
    [SerializeField] private Button startBattleButton;

    [Header("Sounds")]
    [SerializeField] private AudioClip buttonClickSound;


    private List<TeamCardView> randomlyGeneratedTeamSlots;
    private List<PlayerData> unlockedPlayers;

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
        ServiceLocator.Instance.GameService.ClearSelectedTeam();

        randomlyGeneratedTeamSlots = TeamPlayerSlots
            .Where(teamPlayerSlot => teamPlayerSlot != null)
            .Take(RandomTeamSize)
            .ToList();

        if (randomlyGeneratedTeamSlots.Count < RandomTeamSize)
        {
            Debug.LogError("Not enough team slots to generate a random team.");
            return;
        }

        unlockedPlayers = AllPlayers
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

            randomlyGeneratedTeamSlots[i].AddToTeam(randomPlayer, i);
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
    }


    private void OnStartBattleButtonClick()
    {
        ServiceLocator.Instance.SoundService.PlaySound(buttonClickSound);
        if (HasGeneratedTeam())
            SceneManager.LoadScene(1);
            
    }

    private bool HasGeneratedTeam()
    {
        return randomlyGeneratedTeamSlots != null
            && randomlyGeneratedTeamSlots.Count == RandomTeamSize
            && randomlyGeneratedTeamSlots.All(teamSlot => teamSlot != null && teamSlot.data != null);
    }

}
