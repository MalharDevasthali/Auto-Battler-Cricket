using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerData data;

    [Header("UI Elements")]
    [SerializeField] private RandomTeamGenerator teamSelectionController;
    [SerializeField] private Image selectedImage;
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI battingPowerText;
    [SerializeField] private TextMeshProUGUI bowlingPowerText;
    [SerializeField] private TextMeshProUGUI defenceText;
    [SerializeField] private RectTransform BattingPowerUI;
    [SerializeField] private RectTransform BowlingPowerUI;


    [SerializeField] private AudioClip buttonClickSound;
    
    private Button button;

    private GameObject dragObject;
    private RectTransform dragRect;
    private Canvas canvas;
    private CanvasGroup canvasGroup;



    private UIService uiService;

    void Awake()
    {
        button = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvas = GetComponentInParent<Canvas>();
 
    }
    private void Start()
    {
        uiService = ServiceLocator.Instance.UIService;
        InitilizeCard();
    }

    private void InitilizeCard()
    {
        playerImage.sprite = data.playerSprite;
        battingPowerText.text = data.BattingPower.ToString();
        bowlingPowerText.text = data.BowlingPower.ToString();
        defenceText.text = data.Defense.ToString();
        
        if(data.role == PlayerRole.Batsman) 
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

  

    private bool CheckPlayerType()
    {
        return data.role == PlayerRole.Batsman && ServiceLocator.Instance.GameService.GetCurrentInnings() == Innings.Batting
                    || data.role == PlayerRole.Bowler && ServiceLocator.Instance.GameService.GetCurrentInnings() == Innings.Bowling;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (data == null) return;
        uiService.ShowPlayerStats(data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (data == null) return;
        uiService.HidePlayerStats();
    }

  
}