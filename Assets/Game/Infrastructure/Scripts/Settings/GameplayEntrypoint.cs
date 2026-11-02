using UnityEngine;
using Zenject;

public class GameplayEntrypoint : MonoBehaviour
{
    private DayCycleService _dayCycleService;
    private PlayerTools _playerTools;
    private PlayerMutagens _playerMutagens;
    private PlayerGrenades _playerGrenades;
    private ChickenSpawner _chickenSpawner;
    private CulturesSelection _culturesSelection;
    
    [Inject]
    private void Initialize(DayCycleService dayCycleService, PlayerTools playerTools, CulturesSelection cultures, PlayerMutagens playerMutagens,
        PlayerGrenades playerGrenades, ChickenSpawner chickenSpawner)
    {
        _dayCycleService = dayCycleService;
        _playerTools = playerTools;
        _playerMutagens = playerMutagens;
        _playerGrenades = playerGrenades;
        _chickenSpawner = chickenSpawner;
        _culturesSelection = cultures;
    }

    private void Start()
    {
        _playerTools.Add(ToolType.Rake);
        _playerTools.Add(ToolType.Bailer);
        _playerTools.Add(ToolType.Shotgun);
        _playerTools.Add(ToolType.Shovel);
        
        _dayCycleService.StartDay();
        this.Log("Gameplay started");
    }
}
