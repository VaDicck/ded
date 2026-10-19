using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SleepingBed : MonoBehaviour, IInteractable
{
    public bool IsInteractable => !_dayCycleService.IsDay;
    public bool IsUsable => false;
    
    private DayCycleService _dayCycleService;
        
    public UniTask Interact()
    {
        _dayCycleService.StartDay();
        
        return UniTask.CompletedTask;
    }

    [Inject]
    private void Init(DayCycleService dayCycleService)
    {
        _dayCycleService = dayCycleService;
    }
}
