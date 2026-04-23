using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RandomTeamGenerator : MonoBehaviour
{
    private int teamSize;

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
        ServiceLocator.Instance.GameService.ClearSelectedTeam();
        teamSize = ServiceLocator.Instance.GameService.GetUnlockedSlots();

        randomlyGeneratedTeamSlots = TeamPlayerSlots
            .Where(teamPlayerSlot => teamPlayerSlot != null)
            .Take(teamSize)
            .ToList();

        if (randomlyGeneratedTeamSlots.Count < teamSize)
        {
            Debug.LogError("Not enough team slots to generate a random team.");
            return;
        }

        unlockedPlayers = AllPlayers
            .Where(playerCard => playerCard != null && playerCard.data != null)
            .Select(playerCard => playerCard.data)
            .Distinct()
            .ToList();

        if (unlockedPlayers.Count < teamSize)
        {
            Debug.LogError("Not enough unlocked players to generate a random team.");
            return;
        }

        for (int i = 0; i < teamSize; i++)
        {
            int randomIndex = Random.Range(0, unlockedPlayers.Count);
            PlayerData randomPlayer = unlockedPlayers[randomIndex];
            unlockedPlayers.RemoveAt(randomIndex);

            randomlyGeneratedTeamSlots[i].AddToTeam(randomPlayer, i);
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
            && randomlyGeneratedTeamSlots.Count == teamSize
            && randomlyGeneratedTeamSlots.All(teamSlot => teamSlot != null && teamSlot.data != null);
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

}
