using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class RockingChair : MonoBehaviour, IInteractable
{
    public bool IsInteractable => _dayCycleService.IsDay;
    public bool IsUsable => false;
    
    private DayCycleService _dayCycleService;
    
    [Inject]
    private void Init(DayCycleService dayCycleService)
    {
        _dayCycleService = dayCycleService;
    }
    
    public UniTask Interact()
    {
        _dayCycleService.StartNight();
        
        return UniTask.CompletedTask;
    }
}
