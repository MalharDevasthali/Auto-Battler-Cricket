using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLineupView : MonoBehaviour
{

    private UIService uiService;
    private PlayerData data;

    private void Awake()
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
}
