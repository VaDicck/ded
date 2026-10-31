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

    private void Awake()
    {
        _alienService.AliensOver += Leave;
        
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(ArriveCoroutine(_timeAnimation+_delayStartAnimation));
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
        gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        _alienService.AliensOver -= Leave;
    }
}
