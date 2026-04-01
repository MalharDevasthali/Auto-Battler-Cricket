using Unity.VisualScripting;
using UnityEngine;

public class ServiceLocator : MonoSingleton<ServiceLocator>
{

    [Header("Services")]
    [SerializeField] private GameService gameService;
    [SerializeField] private SoundService soundService;
    [SerializeField] private UIService uiService;

    public GameService GameService => gameService;
    public SoundService SoundService => soundService;
    public UIService UIService => uiService;

    protected override void Awake()
    {
        base.Awake(); 
    }

}
