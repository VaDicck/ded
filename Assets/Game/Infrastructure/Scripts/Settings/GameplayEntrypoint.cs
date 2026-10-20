using UnityEngine;
using Zenject;

public class GameplayEntrypoint : MonoBehaviour
{
    private DayCycleService _dayCycleService;
    private PlayerTools _playerTools;
    
    [Inject]
    private void Initialize(DayCycleService dayCycleService, PlayerTools playerTools)
    {
        _dayCycleService = dayCycleService;
        _playerTools = playerTools;
    }

    private void Start()
    {
        _playerTools.Add(ToolType.Shovel);
        _playerTools.Add(ToolType.Bailer);
        _playerTools.Add(ToolType.Shotgun);
        _playerTools.Add(ToolType.Pickaxe);
        _playerTools.Add(ToolType.Axe);
        _dayCycleService.StartDay();
        this.Log("Gameplay started");
    }
}
