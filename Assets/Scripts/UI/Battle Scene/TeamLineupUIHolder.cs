using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamLineupUIHolder : MonoBehaviour
{
    [SerializeField] private List<PlayerLineupView> playerLineUpList = new List<PlayerLineupView>();
    [SerializeField] private List<PlayerData> placeHolderTeam = new List<PlayerData>();

    public List<PlayerLineupView> GetPlayerLineupList() => playerLineUpList;


    private void Start()
    {

        InitilizeTeamLineUp();
    }

    public void PopulatePlayers()
    {
        for (int i = 0; i < playerLineUpList.Count; i++)
        {
            if (playerLineUpList[i].GetData() == null)
            {
                playerLineUpList[i].SetPlayerData(placeHolderTeam[i]);
                playerLineUpList[i].LoadUI();
            }
            else
                Debug.Log("Player Data Already Exists");
        }
    }

    private void InitilizeTeamLineUp()
    {
        List<PlayerData> selectedTeamData = ServiceLocator.Instance.GameService.GetSelectedTeam();
        if (selectedTeamData == null) return;

        Debug.Log("Selected Team Data List Count:"+selectedTeamData.Count);
        Debug.Log("PlayerLineup List Count:" + playerLineUpList.Count);

        for (int i = 0; i < playerLineUpList.Count; i++)
        {
            if (playerLineUpList[i].GetData() == null)
            {
                playerLineUpList[i].SetPlayerData(selectedTeamData[i]);
                playerLineUpList[i].LoadUI();
            }
            else
                Debug.Log("Player Data Already Exists");
        }
    }

    public void ResetTeamLineUp()
    {
        for (int i = 0; i < playerLineUpList.Count; i++)
        {
            
           playerLineUpList[i].LoadUI();
           playerLineUpList[i].SetCurrentPlayerIndicator(false);
        }
    }


}
