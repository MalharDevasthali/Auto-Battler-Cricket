using Mono.Cecil;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamCardView : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler , IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public PlayerData data;
    private int slotIndex;

    [Header("UI Elements")]
    [SerializeField] private RandomTeamGenerator randomTeamGenerator;
    [SerializeField] private TextMeshProUGUI battingPowerText;
    [SerializeField] private TextMeshProUGUI bowlingPowerText;
    [SerializeField] private TextMeshProUGUI defenceText;
    [SerializeField] private RectTransform BattingPowerUI;
    [SerializeField] private RectTransform BowlingPowerUI;
    [SerializeField] private RectTransform DefenceUI;

    [Header("Sounds")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip cardSwipeSound;


    private Button button;
    private UIService uiService;

    private GameObject dragObject;
    private RectTransform dragRect;
    private Canvas canvas;
    private CanvasGroup canvasGroup;



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
    }

    public void AddToBattingLineup(PlayerData data, int slotIndex)
    {
        this.data = data;
        this.slotIndex = slotIndex;
        ServiceLocator.Instance.GameService.AddBatsman(this.data, slotIndex);
        GetComponent<Image>().sprite = data.playerSprite;
        LoadUIForCard(data);
    }
    public void AddToBowlingLineup(PlayerData data)
    {
        this.data = data;
        ServiceLocator.Instance.GameService.AddBowler(this.data);
        GetComponent<Image>().sprite = data.playerSprite;
        LoadUIForCard(data);
    }

    public void RemoveFromBattingLineup()
    {
        BattingPowerUI.gameObject.SetActive(false);
        BowlingPowerUI.gameObject.SetActive(false);
        DefenceUI.gameObject.SetActive(false);
        ServiceLocator.Instance.GameService.RemoveBatsman(this.data,slotIndex);
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
        ServiceLocator.Instance.SoundService.PlaySound(cardSwipeSound);
        if (eventData.pointerEnter != null)
        {
            TeamCardView targetCard = eventData.pointerEnter.GetComponentInParent<TeamCardView>();

            if (targetCard != null && targetCard != this)
            {
                // Move  if target is empty
                if (targetCard.data == null && this.data != null)
                {
                    PlayerData tempPlayerData = this.data;
                    //  randomTeamGenerator.RemovePlayer(data);
                    //  randomTeamGenerator.AddPlayer(tempPlayerData, targetCard.slotIndex);

                }
                //Swap if both have data
                if (targetCard.data != null && this.data != null)
                {
                    randomTeamGenerator.SwapPlayers(this.slotIndex, targetCard.slotIndex);
                }
            }
        }

        if (dragObject != null)
        {
            Destroy(dragObject);
        }
    }


}