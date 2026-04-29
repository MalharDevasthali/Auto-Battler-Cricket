using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RandomTeamGenerator : MonoBehaviour
{
    private int batsmanSlotsSize;

    [Header("UI")]
    [SerializeField] private List<TeamCardView> batsmanPlayerSlots = new List<TeamCardView>();
    [SerializeField] private TeamCardView bowlerPlayerSlot;
    [SerializeField] private List<PlayerCardView> AllPlayers = new List<PlayerCardView>();
    [SerializeField] private Button startBattleButton;


    [Header("Sounds")]
    [SerializeField] private AudioClip buttonClickSound;


    private List<TeamCardView> randomlyGeneratedTeamSlots;
    private List<PlayerData> unlockedBatsmans;
    private List<PlayerData> unlockedBowlers;

    private void Awake()
    {
        if (startBattleButton != null)
            startBattleButton.onClick.AddListener(OnStartBattleButtonClick);
    }

  

    private IEnumerator Start()
    {
        yield return null;
        generateRandomBatsmen();
        generateRandomBowler();
    }

    private void generateRandomBatsmen()
    {
        ServiceLocator.Instance.GameService.ClearSelectedTeam();
        batsmanSlotsSize = ServiceLocator.Instance.GameService.GetUnlockedSlots();

        randomlyGeneratedTeamSlots = batsmanPlayerSlots
            .Where(teamPlayerSlot => teamPlayerSlot != null)
            .Take(batsmanSlotsSize)
            .ToList();

        if (randomlyGeneratedTeamSlots.Count < batsmanSlotsSize)
        {
            Debug.LogError("Not enough team slots to generate a random team.");
            return;
        }

        unlockedBatsmans = AllPlayers
            .Where(playerCard => playerCard != null && playerCard.data != null && playerCard.data.role == PlayerRole.Batsman)
            .Select(playerCard => playerCard.data)
            .Distinct()
            .ToList();

        if (unlockedBatsmans.Count < batsmanSlotsSize)
        {
            Debug.LogError("Not enough unlocked players to generate a random team.");
            return;
        }

        for (int i = 0; i < batsmanSlotsSize; i++)
        {
            int randomIndex = Random.Range(0, unlockedBatsmans.Count);
            PlayerData randomBatsman = unlockedBatsmans[randomIndex];
            unlockedBatsmans.RemoveAt(randomIndex);

            randomlyGeneratedTeamSlots[i].AddToBattingLineup(randomBatsman, i);
        }
    }

    private void generateRandomBowler()
    {
        if (bowlerPlayerSlot == null)
        {
            Debug.LogError("Bowler team slot is missing.");
            return;
        }

        unlockedBowlers = AllPlayers
            .Where(playerCard => playerCard != null && playerCard.data != null && playerCard.data.role == PlayerRole.Bowler)
            .Select(playerCard => playerCard.data)
            .Distinct()
            .ToList();

        if (unlockedBowlers.Count == 0)
        {
            Debug.LogError("No unlocked bowlers available to generate a random bowler.");
            return;
        }

        int randomIndex = Random.Range(0, unlockedBowlers.Count);
        PlayerData randomBowler = unlockedBowlers[randomIndex];
        bowlerPlayerSlot.AddToBowlingLineup(randomBowler);
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
            && randomlyGeneratedTeamSlots.Count == batsmanSlotsSize
            && randomlyGeneratedTeamSlots.All(teamSlot => teamSlot != null && teamSlot.data != null);
    }

    public void SwapPlayers(int fromIndex, int toIndex)
    {
        var fromData = batsmanPlayerSlots[fromIndex].data;
        var toData = batsmanPlayerSlots[toIndex].data;
        
        batsmanPlayerSlots[fromIndex].RemoveFromBattingLineup();
        batsmanPlayerSlots[toIndex].RemoveFromBattingLineup();
       
        if (fromData != null)
            batsmanPlayerSlots[toIndex].AddToBattingLineup(fromData, toIndex);
        if (toData != null)
            batsmanPlayerSlots[fromIndex].AddToBattingLineup(toData, fromIndex);
    }

}
