using Mono.Cecil;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamCardView : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
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
    private int playingOrder;

    private GameObject dragObject;
    private RectTransform dragRect;
    private Canvas canvas;
    private CanvasGroup canvasGroup;



    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickCard);

        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvas = GetComponentInParent<Canvas>();
    }
    private void Start()
    {
        uiService = ServiceLocator.Instance.UIService;
    }
    public void AddToTeam(PlayerData data, int playingOrder)
    {
        this.data = data;
        this.playingOrder = playingOrder;
        ServiceLocator.Instance.GameService.AddPlayerData(this.data, playingOrder);
        GetComponent<Image>().sprite = data.playerSprite;
        LoadUIForCard(data);
    }
    public void RemoveFromTeam()
    {
        GetComponent<Image>().sprite = emptySlotSprite;
        BattingPowerUI.gameObject.SetActive(false);
        BowlingPowerUI.gameObject.SetActive(false);
        DefenceUI.gameObject.SetActive(false);
        ServiceLocator.Instance.GameService.RemovePlayerData(this.data,playingOrder);
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        ServiceLocator.Instance.SoundService.PlaySound(buttonClickSound);

        dragObject = new GameObject("DragImage", typeof(RectTransform), typeof(Image));
        dragObject.transform.SetParent(canvas.transform, false);


        Image img = dragObject.GetComponent<Image>();
        img.sprite = data.playerSprite ;
        img.raycastTarget = false; // IMPORTANT


        dragRect = dragObject.GetComponent<RectTransform>();
        dragRect.sizeDelta = this.gameObject.GetComponent<Image>().rectTransform.sizeDelta;


        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 pos
        );

        dragRect.anchoredPosition = pos;
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