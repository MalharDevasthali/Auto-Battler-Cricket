using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCardView : MonoBehaviour
{
    public PlayerData data;

    [Header("UI Elements")]
    [SerializeField]private Image selectedImage;
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI battingPowerText;
    [SerializeField] private TextMeshProUGUI bowlingPowerText;
    [SerializeField] private TextMeshProUGUI defenceText;
    [SerializeField] private RectTransform BattingPowerUI;
    [SerializeField] private RectTransform BowlingPowerUI;


    [SerializeField] private AudioClip buttonClickSound;
    
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
        InitilizeCard();
    }

    private void InitilizeCard()
    {
        playerImage.sprite = data.cardSprite;
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

}