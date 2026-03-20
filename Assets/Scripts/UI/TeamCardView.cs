using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamCardView : MonoBehaviour
{
    public PlayerData data;
    [SerializeField] private Sprite emptySlotSprite;
    
    private Button button;
    private TeamSelectionService teamSelectionService;

    void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        teamSelectionService = ServiceLocator.Instance.TeamSelectionService;
    }
    public void AddToTeam(PlayerData data, int playingOrder)
    {
        this.data = data;
        GetComponent<Image>().sprite = data.cardSprite;
    }
    public void RemoveFromTeam()
    {
        GetComponent<Image>().sprite = emptySlotSprite;
        data = null;
    }
    
}