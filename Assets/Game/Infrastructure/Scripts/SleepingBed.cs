using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SleepingBed : MonoBehaviour, IInteractable
{
    public bool IsInteractable => !_dayCycleService.IsDay && !_alienService.WaveActive;
    public bool IsUsable => false;
    
    [Inject] private DayCycleService _dayCycleService;
    [Inject] private AlienService _alienService;

    public UniTask Interact()
    {
        _dayCycleService.StartDay();
        
        return UniTask.CompletedTask;
    }
}
