using System.Collections;
using System;
using UnityEngine;
using Zenject;

public class Ufo : MonoBehaviour
{
    public event Action Arrived;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private float _timeAnimation;
    [SerializeField] private float _delayStartAnimation = 2f;
    
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _flySound;
    [SerializeField] private AudioClip _spawnSound;
    
    [Inject] private AlienService _alienService;
    [Inject] private DayCycleService _dayCycleService;

    private void Awake()
    {
        _alienService.WaveOver += Leave;
        _dayCycleService.DayStarted += Hide;
        
        Hide();
    }

    private void OnEnable()
    {
        _audioSource.clip = _flySound;
        _audioSource.Play();
        StartCoroutine(ArriveCoroutine(_timeAnimation+_delayStartAnimation));
    }

    public void Spawn()
    {
        _audioSource.PlayOneShot(_spawnSound);
    }

    private void Hide()
    {
        _audioSource.Stop();
        gameObject.SetActive(false);
    }

    private void Leave()
    {
        _animator.SetTrigger("Leave");
        
        StartCoroutine(LeaveCoroutine(_timeAnimation+_delayStartAnimation));
    }
    
    private IEnumerator ArriveCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Arrived?.Invoke();
    }
    
    private IEnumerator LeaveCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Hide();
    }
    
    private void OnDestroy()
    {
        _alienService.WaveOver -= Leave;
        _dayCycleService.DayStarted += Hide;
    }
}
