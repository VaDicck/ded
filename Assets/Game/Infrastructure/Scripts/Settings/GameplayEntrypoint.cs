using UnityEngine;
using Zenject;

public class GameplayEntrypoint : MonoBehaviour
{
    private DayCycleService _dayCycleService;
    
    [Inject]
    private void Initialize(DayCycleService dayCycleService)
    {
        _dayCycleService = dayCycleService;
    }

    private void Start()
    {
        _dayCycleService.StartDay();
        this.Log("Gameplay started");
    }
}
