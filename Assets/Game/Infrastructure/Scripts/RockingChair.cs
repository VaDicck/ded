using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class RockingChair : MonoBehaviour, IInteractable
{
    public bool IsInteractable => _dayCycleService.IsDay;
    public bool IsUsable => false;
    
    private DayCycleService _dayCycleService;
    private TimeFadeService _fadeService;
    
    [Inject]
    private void Init(DayCycleService dayCycleService, TimeFadeService fadeService)
    {
        _dayCycleService = dayCycleService;
        _fadeService = fadeService;
    }
    
    public UniTask Interact()
    {
        _fadeService.Fade("Ночь", () => _dayCycleService.StartNight());
        
        return UniTask.CompletedTask;
    }
}
