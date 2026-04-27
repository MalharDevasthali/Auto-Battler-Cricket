using System;
using UnityEngine;
using UnityEngine.UI;

public class TossManager : MonoBehaviour
{
    [SerializeField] private Button tossButton;


    private void Awake()
    {
        if (tossButton != null)
            tossButton.onClick.AddListener(OnTossButtonClick);
    }

    private void OnTossButtonClick()
    {
       
    }
}
