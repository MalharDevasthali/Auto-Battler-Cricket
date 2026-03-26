using Unity.VisualScripting;
using UnityEngine;

public class ServiceLocator : MonoSingleton<ServiceLocator>
{

    [Header("Services")]
    [SerializeField] private TeamSelectionService teamSelectionService;
    [SerializeField] private GameService gameService;
    [SerializeField] private SoundService soundService;
    [SerializeField] private UIService uiService;

    public TeamSelectionService TeamSelectionService => teamSelectionService;
    public GameService GameService => gameService;
    public SoundService SoundService => soundService;
    public UIService UIService => uiService;

    protected override void Awake()
    {
        base.Awake();

        ValidateServices();
    }

    private void ValidateServices()
    {
        if (teamSelectionService == null)
            Debug.LogError("TeamSelectionService is NOT assigned in ServiceLocator!");

        if (gameService == null)
            Debug.LogError("GameService is NOT assigned in ServiceLocator!");
    }
}
