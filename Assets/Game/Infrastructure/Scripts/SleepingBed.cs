using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SleepingBed : MonoBehaviour, IInteractable
{
    public bool IsInteractable => !_dayCycleService.IsDay && !_alienService.WaveActive;
    public bool IsUsable => false;
    
    [SerializeField] private AudioClip _sleepSound;
    
    [Inject] private DayCycleService _dayCycleService;
    [Inject] private DayService _dayService;
    [Inject] private AlienService _alienService;
    [Inject] private AudioService _audioService;
    [Inject] private TimeFadeService _fadeService;

    public UniTask Interact()
    {
        _fadeService.Fade($"{_dayService.Day+1} День", () => _dayCycleService.StartDay());

        _audioService.PlaySfx(_sleepSound);
        
        return UniTask.CompletedTask;
    }
}
