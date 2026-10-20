using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SleepingBed : MonoBehaviour, IInteractable
{
    public bool IsInteractable => !_dayCycleService.IsDay && _alienService.AliensEmpty;
    public bool IsUsable => false;
    
    private DayCycleService _dayCycleService;
    private AlienService _alienService;
        
    public UniTask Interact()
    {
        _dayCycleService.StartDay();
        
        return UniTask.CompletedTask;
    }

    [Inject]
    private void Init(DayCycleService dayCycleService, AlienService alienService)
    {
        _dayCycleService = dayCycleService;
        _alienService = alienService;
    }
}
