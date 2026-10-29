using UnityEngine;
using Zenject;

public class GameplayEntrypoint : MonoBehaviour
{
    private DayCycleService _dayCycleService;
    private PlayerTools _playerTools;
    private PlayerMutagens _playerMutagens;
    
    [Inject]
    private void Initialize(DayCycleService dayCycleService, PlayerTools playerTools, PlayerMutagens playerMutagens)
    {
        _dayCycleService = dayCycleService;
        _playerTools = playerTools;
        _playerMutagens = playerMutagens;
    }

    private void Start()
    {
        _playerTools.Add(ToolType.Rake);
        _playerTools.Add(ToolType.Bailer);
        _playerTools.Add(ToolType.Shotgun);
        _playerTools.Add(ToolType.Pickaxe);
        _playerTools.Add(ToolType.Mutagen);
        
        _playerMutagens.Add();
        
        _dayCycleService.StartDay();
        this.Log("Gameplay started");
    }
}
