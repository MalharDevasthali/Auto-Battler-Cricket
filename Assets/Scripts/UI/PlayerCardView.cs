using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public PlayerData data;

    [Header("UI Elements")]
    [SerializeField] private TeamSelectionController teamSelectionService;
    [SerializeField] private Image selectedImage;
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI battingPowerText;
    [SerializeField] private TextMeshProUGUI bowlingPowerText;
    [SerializeField] private TextMeshProUGUI defenceText;
    [SerializeField] private RectTransform BattingPowerUI;
    [SerializeField] private RectTransform BowlingPowerUI;


    [SerializeField] private AudioClip buttonClickSound;
    
    private Button button;
    private bool isSelected = false;


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
        //  button.onClick.AddListener(OnClickCard);
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

    void OnClickCard()
    {
        ServiceLocator.Instance.SoundService.PlaySound(buttonClickSound);
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

    private void Select()
    {
        isSelected = true;
        selectedImage.enabled = true;
        transform.localScale = Vector3.one * 1.1f;
    }

    public void Deselect()
    {
        isSelected = false;
        selectedImage.enabled = false;
        transform.localScale = Vector3.one;
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragObject = new GameObject("DragImage", typeof(RectTransform), typeof(Image));
        dragObject.transform.SetParent(canvas.transform, false);

    
        Image img = dragObject.GetComponent<Image>();
        img.sprite = playerImage.sprite;
        img.raycastTarget = false; // IMPORTANT

        // Match size
        dragRect = dragObject.GetComponent<RectTransform>();
        dragRect.sizeDelta = playerImage.rectTransform.sizeDelta;

        // Set position at mouse
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 pos
        );

        dragRect.anchoredPosition = pos;

        // Bring to front
        dragRect.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragRect == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 pos
        );

        dragRect.anchoredPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(dragObject);
    }
}