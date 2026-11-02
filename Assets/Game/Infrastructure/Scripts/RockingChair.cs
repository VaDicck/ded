using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class RockingChair : MonoBehaviour, IInteractable
{
    public event Action Interacted;
    
    public bool IsInteractable => _dayCycleService.IsDay;
    public bool IsUsable => false;
    
    private DayCycleService _dayCycleService;
    private TimeFadeService _fadeService;
    private AudioService _audioService;
    
    [SerializeField] private AudioClip _grandpaSleepSound;
    [SerializeField, Range(0,1)] private float _sleepVolume;
    
    [Inject]
    private void Init(DayCycleService dayCycleService, TimeFadeService fadeService, AudioService audioService)
    {
        _dayCycleService = dayCycleService;
        _fadeService = fadeService;
        _audioService = audioService;
    }
    
    public UniTask Interact()
    {
        _fadeService.Fade("Ночь", () =>
        {
            _dayCycleService.StartNight();
            Interacted?.Invoke();
        });
        
        _audioService.PlaySfx(_grandpaSleepSound, _sleepVolume);
        
        return UniTask.CompletedTask;
    }
}
