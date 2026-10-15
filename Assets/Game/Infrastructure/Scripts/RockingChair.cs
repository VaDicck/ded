using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class RockingChair : MonoBehaviour, IInteractable
{
    public bool IsInteractable => _isInteractable;
    public bool IsUsable => false;
    
    private DayCycleService _dayCycleService;
    private bool _isInteractable = true;
    
    [Inject]
    private void Init(DayCycleService dayCycleService)
    {
        _dayCycleService = dayCycleService;
    }

    public void Awake()
    {
        _dayCycleService.DayStarted += () => _isInteractable = true;
    }
    
    public UniTask Interact()
    {
        _dayCycleService.StartNight();
        
        _isInteractable = false;
        
        return UniTask.CompletedTask;
    }
}
