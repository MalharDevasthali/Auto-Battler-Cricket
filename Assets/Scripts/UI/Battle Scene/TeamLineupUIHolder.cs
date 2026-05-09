using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamLineupUIHolder : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private PlayerLineupView playerLineupPrefab;
    [SerializeField] private Transform teamPlayersParent;
    private List<PlayerLineupView> playerLineUpList = new List<PlayerLineupView>();

    [Header("Debug")]
    [SerializeField] private List<PlayerData> placeHolderTeam = new List<PlayerData>();

    private bool isInitialized;

    public List<PlayerLineupView> GetPlayerLineupList( Innings currentInnings , int matchNumber)
    {
        InitilizeTeamLineUp(currentInnings,matchNumber);
        return playerLineUpList;
    }

    private void InitilizeTeamLineUp(Innings currentInnings,int matchNumber)
    {
        List<PlayerData> randombatsmanTeam;
        if (currentInnings == Innings.Batting)
        {
            randombatsmanTeam = ServiceLocator.Instance.GameService.GetPlayerBatmanTeam();
        }
        else
        {
            randombatsmanTeam = ServiceLocator.Instance.GameService.GetCPUBatsmanTeam(matchNumber);
        }

        CreateLineup(randombatsmanTeam);
    }

    private void CreateLineup(List<PlayerData> teamData)
    {

        List<PlayerData> playersToShow = teamData
            .Where(playerData => playerData != null)
            .ToList();

        PlayerLineupView template = GetLineupTemplate();
        ClearChildren(teamPlayersParent);
        playerLineUpList.Clear();

        for (int i = 0; i < playersToShow.Count; i++)
        {
            PlayerLineupView playerLineupView = Instantiate(template, teamPlayersParent);
            playerLineupView.name = "PlayerLineup_" + (i + 1);
            playerLineupView.SetPlayerData(playersToShow[i]);
            playerLineupView.LoadUI();
            playerLineupView.SetCurrentPlayerIndicator(false);
            playerLineUpList.Add(playerLineupView);
        }
        Debug.Log("PlayerLineup List Count:" + playerLineUpList.Count);
    }
    private void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    private PlayerLineupView GetLineupTemplate()
    {
         return playerLineupPrefab;
    }

    public void ResetTeamLineUp()
    {
        for (int i = 0; i < playerLineUpList.Count; i++)
        {
            if (playerLineUpList[i] == null) continue;

            playerLineUpList[i].LoadUI();
            playerLineUpList[i].SetCurrentPlayerIndicator(false);
        }
    }
}
