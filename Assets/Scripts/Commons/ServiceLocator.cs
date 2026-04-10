using Unity.VisualScripting;
using UnityEngine;

public class ServiceLocator : MonoSingleton<ServiceLocator>
{

    [Header("Services")]
    [SerializeField] private GameService gameService;
    [SerializeField] private SoundService soundService;
    [SerializeField] private UIService uiService;
    [SerializeField] private EventService eventService;

    public GameService GameService => gameService;
    public SoundService SoundService => soundService;
    public UIService UIService => uiService;
    public EventService EventService => eventService;

    protected override void Awake()
    {
        base.Awake(); 
    }

}
