using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Train : MonoBehaviour
{
    [SerializeField] private List<Carriage> _carriages;
    [SerializeField] private Animator _animator;

    [Header("Audio")]
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _arriveClip;
    [SerializeField] private AudioClip _leaveClip;
    [SerializeField] private AudioClip _idleClip;

    private DayCycleService _dayCycleService;

    private bool _left = true;
    
    [Inject]
    private void Init(DayCycleService dayCycle)
    {
        _dayCycleService = dayCycle;
    }

    private void Awake()
    {
        _dayCycleService.DayStarted += Arrive;
        _dayCycleService.NightStarted += Leave;
    }

    private void Start()
    {
        Arrive();
    }

    private void Arrive()
    {
        if (_left == false)
            return;
        
        _left = false;
        Hide();
        Show();
    }

    private void Leave()
    {
        if (_left)
            return;
        
        _left = true;
        
        _audio.PlayOneShot(_leaveClip);
        
        _animator.SetTrigger("Leave");
        
        _animator.SetTrigger("Moving");
    }

    private void Arrived()
    {
        _animator.SetTrigger("NotMoving");
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void Show()
    {
        gameObject.SetActive(true);
        _audio.clip = _idleClip;
        _audio.Play();
        _audio.PlayOneShot(_arriveClip);

        foreach (var carriage in _carriages)
        {
            if(carriage.Show())
                carriage.QuotaReached += CheckQuotaReached;
        }
    }

    private void CheckQuotaReached()
    {
        bool AllСarriageReached = true;
        
        foreach (var carriage in _carriages)
        {
            if (!carriage.QuotaElementReached)
                AllСarriageReached = false;
        }

        if (AllСarriageReached)
        {
            Leave();
        }
    }
    
    private void OnDestroy()
    {
        _dayCycleService.DayStarted -= Arrive;
        _dayCycleService.NightStarted -= Leave;
    }
}
