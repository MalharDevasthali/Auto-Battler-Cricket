using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class PlayerCardView : MonoBehaviour
{
    public PlayerData data;
    private Button button;
    private bool isSelected = false;

    private TeamSelectionService teamSelectionService;

    void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClickCard);
    }
    private void Start()
    {
        teamSelectionService = ServiceLocator.Instance.TeamSelectionService;
    }
   
    void OnClickCard()
    {
        if (CheckPlayerType())
        {
            if (!isSelected)
            {
                if (teamSelectionService.CanSelect(data.role))
                {
                    Select();
                    teamSelectionService.AddPlayer(this.data);
                }
            }
            else
            {
                Deselect();
                teamSelectionService.RemovePlayer(this.data);
            }
        }
        else
        {
            Debug.Log("Current Inning is: " + ServiceLocator.Instance.GameService.GetCurrentInnings() + " Please Select Appropriate Player");
        }
    }

    private bool CheckPlayerType()
    {
        return data.role == PlayerRole.Batsman && ServiceLocator.Instance.GameService.GetCurrentInnings() == GameService.Innings.Batting
                    || data.role == PlayerRole.Bowler && ServiceLocator.Instance.GameService.GetCurrentInnings() == GameService.Innings.Bowling;
    }

    public void Select()
    {
        isSelected = true;
        transform.localScale = Vector3.one * 1.1f;
    }

    public void Deselect()
    {
        isSelected = false;
        transform.localScale = Vector3.one;
    }

}