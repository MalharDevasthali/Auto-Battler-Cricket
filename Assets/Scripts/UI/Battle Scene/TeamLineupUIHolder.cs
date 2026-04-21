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

    public List<PlayerLineupView> GetPlayerLineupList()
    {
        EnsureInitialized();
        return playerLineUpList;
    }

    private void Start()
    {
        EnsureInitialized();
    }

    public void PopulatePlayers()
    {
        CreateLineup(placeHolderTeam);
        isInitialized = true;
    }

    private void InitilizeTeamLineUp()
    {
        List<PlayerData> selectedTeamData = ServiceLocator.Instance.GameService.GetSelectedTeam();
        if (selectedTeamData == null) return;

        Debug.Log("Selected Team Data List Count:" + selectedTeamData.Count);
        CreateLineup(selectedTeamData);
    }

    private void EnsureInitialized()
    {
        if (isInitialized) return;

        InitilizeTeamLineUp();
        isInitialized = true;
    }

    private void CreateLineup(List<PlayerData> teamData)
    {
        if (teamData == null) return;

        List<PlayerData> playersToShow = teamData
            .Where(playerData => playerData != null)
            .ToList();

        PlayerLineupView template = GetLineupTemplate();
        Transform parent = GetLineupParent(template);

        if (template == null)
        {
            Debug.LogError("PlayerLineupView prefab or template is missing.");
            return;
        }

        if (parent == null)
        {
            Debug.LogError("Team players parent is missing.");
            return;
        }

        List<PlayerLineupView> existingLineupViews = parent
            .GetComponentsInChildren<PlayerLineupView>(true)
            .ToList();

        playerLineUpList.Clear();

        for (int i = 0; i < playersToShow.Count; i++)
        {
            PlayerLineupView playerLineupView = Instantiate(template, parent);
            playerLineupView.name = "PlayerLineup_" + (i + 1);
            playerLineupView.SetPlayerData(playersToShow[i]);
            playerLineupView.LoadUI();
            playerLineupView.SetCurrentPlayerIndicator(false);
            playerLineUpList.Add(playerLineupView);
        }

        foreach (PlayerLineupView existingLineupView in existingLineupViews)
        {
            if (existingLineupView != null)
                Destroy(existingLineupView.gameObject);
        }

        Debug.Log("PlayerLineup List Count:" + playerLineUpList.Count);
    }

    private PlayerLineupView GetLineupTemplate()
    {
        if (playerLineupPrefab != null)
            return playerLineupPrefab;

        PlayerLineupView existingLineupView = playerLineUpList.FirstOrDefault(lineupView => lineupView != null);
        if (existingLineupView != null)
            return existingLineupView;

        if (teamPlayersParent != null)
            return teamPlayersParent.GetComponentInChildren<PlayerLineupView>(true);

        return GetComponentInChildren<PlayerLineupView>(true);
    }

    private Transform GetLineupParent(PlayerLineupView template)
    {
        if (teamPlayersParent != null)
            return teamPlayersParent;

        PlayerLineupView existingLineupView = playerLineUpList.FirstOrDefault(lineupView => lineupView != null);
        if (existingLineupView != null && existingLineupView.transform.parent != null)
            return existingLineupView.transform.parent;

        PlayerLineupView childLineupView = GetComponentInChildren<PlayerLineupView>(true);
        if (childLineupView != null && childLineupView.transform.parent != null)
            return childLineupView.transform.parent;

        if (template != null && template.transform.parent != null)
            return template.transform.parent;

        return transform;
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
