using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerLineupView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("UI Elements")]
    
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI battingPowerText;
    [SerializeField] private TextMeshProUGUI defenceText;
  

    private UIService uiService;
    private PlayerData data;

    private void Start()
    {
        uiService = ServiceLocator.Instance.UIService;
    }

    public void SetPlayerData(PlayerData data)
    {
        this.data = data;
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

    public void LoadUI()
    {
        if (data == null)
        {
            Debug.LogError("PlayerLineupView Data is Null");
            return;
        }

        playerImage.sprite = data.playerSprite;
        playerNameText.text = data.playerName.ToString();
        battingPowerText.text = data.BattingPower.ToString();
        defenceText.text = data.Defense.ToString();

    }

    public PlayerData GetData() { return data; }    
}
