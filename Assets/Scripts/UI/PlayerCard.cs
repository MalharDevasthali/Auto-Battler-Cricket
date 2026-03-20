using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    public PlayerData data;

    private Button button;
    private bool isSelected = false;

    [SerializeField]
    private TeamSelectionService service;

    void Awake()
    {
        button = GetComponent<Button>();
        service =  Object.FindFirstObjectByType<TeamSelectionService>();

        button.onClick.AddListener(OnClickCard);
    }

    void OnClickCard()
    {
        if (data.role == PlayerRole.Batsman)
        {
            if (!isSelected)
            {
                if (service.CanSelect(data.role))
                {
                    Select();
                    service.AddPlayer(this);
                }
            }
            else
            {
                Deselect();
                service.RemovePlayer(this);
            }
        }
        else
        {
            Debug.Log("Bowler clicked (handle later if needed)");
        }
    }

    public void Select()
    {
        isSelected = true;
        // Visual feedback (important!)
        transform.localScale = Vector3.one * 1.1f;
    }

    public void Deselect()
    {
        isSelected = false;
        transform.localScale = Vector3.one;
    }

    public bool IsSelected()
    {
        return isSelected;
    }
}