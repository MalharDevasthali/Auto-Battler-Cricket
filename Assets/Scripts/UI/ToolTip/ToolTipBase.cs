using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public abstract class TooltipBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected RectTransform tooltipUI;
    [SerializeField] protected TextMeshProUGUI tooltipText;

    protected abstract string GetMessage();

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipUI == null || tooltipText == null) return;

        tooltipText.text = GetMessage();
        tooltipUI.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipUI == null) return;

        tooltipUI.gameObject.SetActive(false);
    }
}