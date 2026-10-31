using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SleepingBed : MonoBehaviour, IInteractable
{
    public bool IsInteractable => !_dayCycleService.IsDay && _canStartDay;
    public bool IsUsable => false;
    
    [Inject] private DayCycleService _dayCycleService;
    [Inject] private AlienService _alienService;

    private bool _canStartDay;
    
    public UniTask Interact()
    {
        _dayCycleService.StartDay();
        _canStartDay = false;
        
        return UniTask.CompletedTask;
    }

    private void Awake()
    {
        _alienService.AliensOver += OnAliensOver;
    }

    private void OnAliensOver()
    {
        _canStartDay = true;
    }

    private void OnDestroy()
    {
        _alienService.AliensOver -= OnAliensOver;
    }
}
