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
    
    [Inject] private AlienService _alienService;
    [Inject] private DayCycleService _dayCycleService;

    private void Awake()
    {
        _alienService.AliensOver += Leave;
        _dayCycleService.DayStarted += Hide;
        
        Hide();
    }

    private void OnEnable()
    {
        StartCoroutine(ArriveCoroutine(_timeAnimation+_delayStartAnimation));
    }
    
    private void Hide() => gameObject.SetActive(false);

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
        _alienService.AliensOver -= Leave;
        _dayCycleService.DayStarted += Hide;
    }
}
