using System;
using UnityEngine;
using Zenject;

public class AmbientManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _daySound;
    [SerializeField] private AudioClip _nightSound;
    [SerializeField] private AudioClip _alienAttackSound;
    [SerializeField, Range(0,1)] private float _dayVolume;
    [SerializeField, Range(0,1)] private float _nightVolume;
    [SerializeField, Range(0,1)] private float _attackVolume;

    private DayCycleService _dayCycleService;
    private AlienService _alienService;
    
    [Inject]
    private void Init(DayCycleService dayCycleService, AlienService alienService)
    {
        _dayCycleService = dayCycleService;
        _alienService = alienService;
    }

    private void Start()
    {
        _dayCycleService.DayStarted += PlayDaySound;
        _dayCycleService.DayEnded += PlayNightSound;
        _alienService.WaveStart += PlayAlienAttackSound;
        _alienService.WaveOver += StopAlienAttackSound;
    }

    private void PlayDaySound()
    {
        _audioSource.volume = _dayVolume;
        _audioSource.clip = _daySound;
        _audioSource.Play();
    }

    private void PlayNightSound()
    {
        _audioSource.volume = _nightVolume;
        _audioSource.clip = _nightSound;
        _audioSource.Play();
    }

    private void PlayAlienAttackSound()
    {
        _audioSource.volume = _attackVolume;
        _audioSource.clip = _alienAttackSound;
        _audioSource.Play();
    }

    private void StopAlienAttackSound()
    {
        _audioSource.volume = _nightVolume;
        _audioSource.clip = _nightSound;
        _audioSource.Play();
    }

    private void OnDestroy()
    {
        _dayCycleService.DayStarted -= PlayDaySound;
        _dayCycleService.DayEnded -= PlayNightSound;
        _alienService.WaveStart -= PlayAlienAttackSound;
        _alienService.WaveOver -= StopAlienAttackSound;
    }
}
