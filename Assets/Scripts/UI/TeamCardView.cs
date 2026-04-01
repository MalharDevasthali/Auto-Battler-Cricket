using Mono.Cecil;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamCardView : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public PlayerData data;
    public int slotIndex;

    [Header("UI Elements")]
    [SerializeField] private TeamSelectionController teamSelectionController;
    [SerializeField] private Sprite emptySlotSprite;
    [SerializeField] private TextMeshProUGUI battingPowerText;
    [SerializeField] private TextMeshProUGUI bowlingPowerText;
    [SerializeField] private TextMeshProUGUI defenceText;
    [SerializeField] private RectTransform BattingPowerUI;
    [SerializeField] private RectTransform BowlingPowerUI;
    [SerializeField] private RectTransform DefenceUI;

    [Header("Sounds")]
    [SerializeField] private AudioClip buttonClickSound;


    private Button button;
  
    private UIService uiService;

 
    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickCard);
    }
    private void Start()
    {
        uiService = ServiceLocator.Instance.UIService;
    }
    public void AddToTeam(PlayerData data, int playingOrder)
    {
        this.data = data;
        ServiceLocator.Instance.GameService.AddPlayerData(this.data);
        GetComponent<Image>().sprite = data.playerSprite;
        LoadUIForCard(data);
    }
    public void RemoveFromTeam()
    {
        GetComponent<Image>().sprite = emptySlotSprite;
        BattingPowerUI.gameObject.SetActive(false);
        BowlingPowerUI.gameObject.SetActive(false);
        DefenceUI.gameObject.SetActive(false);
        ServiceLocator.Instance.GameService.RemovePlayerData(this.data);
        data = null;
    }

    private void LoadUIForCard(PlayerData data)
    {
        battingPowerText.text = data.BattingPower.ToString();
        bowlingPowerText.text = data.BowlingPower.ToString();
        defenceText.text = data.Defense.ToString();
        DefenceUI.gameObject.SetActive(true);
        if (data.role == PlayerRole.Batsman)
        {
            BattingPowerUI.gameObject.SetActive(true);
            BowlingPowerUI.gameObject.SetActive(false);
        }
        else
        {
            BattingPowerUI.gameObject.SetActive(false);
            BowlingPowerUI.gameObject.SetActive(true);
        }
    }

    private void OnClickCard()
    {
        ServiceLocator.Instance.SoundService.PlaySound(buttonClickSound);
        teamSelectionController.RemovePlayer(data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(data ==null) return;
        uiService.ShowPlayerStats(data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (data == null) return;
        uiService.HidePlayerStats();
    }

}