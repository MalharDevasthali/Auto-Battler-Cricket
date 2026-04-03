using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIService : MonoBehaviour
{
    [Header("Player Details UI")]
    [SerializeField] private RectTransform playerDetailsParentObject; 
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerDetailsText;

    [SerializeField] private TextMeshProUGUI battingPowerText;
    [SerializeField] private TextMeshProUGUI bowlingPowerText;
    [SerializeField] private TextMeshProUGUI defenceText;

    [SerializeField] private RectTransform battingPowerUI;
    [SerializeField] private RectTransform bowlingPowerUI;

   
    public void ShowPlayerStats(PlayerData data)
    {
        InitializeCard(data);
        playerDetailsParentObject.gameObject.SetActive(true);
    }

    public void HidePlayerStats()
    {
        playerDetailsParentObject.gameObject.SetActive(false);
    }

    public void UpdateUIDuringMatch(PlayerDataDuringMatch data)
    {
        playerImage.sprite = data.playerSprite;
        playerNameText.text = data.playerName;
        playerDetailsText.text = data.SpecialAbility;
        battingPowerText.text = data.BattingPower.ToString();
        bowlingPowerText.text = data.BowlingPower.ToString();
        defenceText.text = data.Defense.ToString();

        playerDetailsParentObject.gameObject.SetActive(true);

    }


    private void InitializeCard(PlayerData data)
    {
        playerImage.sprite = data.playerSprite;

        playerNameText.text = data.playerName;
        playerDetailsText.text = data.SpecialAbility;
        battingPowerText.text = data.BattingPower.ToString();
        bowlingPowerText.text = data.BowlingPower.ToString();
        defenceText.text = data.Defense.ToString();

        if (data.role == PlayerRole.Batsman)
        {
            battingPowerUI.gameObject.SetActive(true);
            bowlingPowerUI.gameObject.SetActive(false);
        }
        else
        {
            battingPowerUI.gameObject.SetActive(false);
            bowlingPowerUI.gameObject.SetActive(true);
        }
    }
}