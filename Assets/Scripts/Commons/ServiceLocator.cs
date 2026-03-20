using Unity.VisualScripting;
using UnityEngine;

public class ServiceLocator : MonoSingleton<ServiceLocator>
{

    [Header("Services")]
    [SerializeField] private TeamSelectionService teamSelectionService;
    [SerializeField] private GameService gameService;

    public TeamSelectionService TeamSelectionService => teamSelectionService;
    public GameService GameService => gameService;

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
