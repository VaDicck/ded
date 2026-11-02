using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class CraftTable : MonoBehaviour, IInteractable
{
    public bool IsInteractable => _isInteractable;
    public bool IsUsable => false;
    
    private bool _isInteractable;

    private CraftTableService _service;
    private DayCycleService _dayCycleService;
    
    [Inject]
    private void Initialize(CraftTableService service, DayCycleService dayCycleService)
    {
        _service = service;
        _dayCycleService = dayCycleService;
    }

    private void Start()
    {
        _dayCycleService.DayStarted += () => _isInteractable = true;
        _dayCycleService.DayEnded += () => _isInteractable = false;
    }
    
    public UniTask Interact()
    {
        _service.Show();
        
        return UniTask.CompletedTask;
    }
}

