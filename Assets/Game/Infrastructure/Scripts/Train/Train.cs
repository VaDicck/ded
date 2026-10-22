using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Train : MonoBehaviour
{
    [SerializeField] private List<Carriage> _carriages;
    [SerializeField] private Animator _animator;

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

    [ContextMenu("Train Arrive")]
    private void Arrive()
    {
        if (_left == false)
            return;
        
        _left = false;
        Hide();
        Show();
        this.Log("Train arrive");
    }

    [ContextMenu("Train Leave")]
    private void Leave()
    {
        if (_left)
            return;
        
        _left = true;
        
        _animator.SetTrigger("Leave");
        
        _animator.SetTrigger("Moving");
        this.Log("Train Leave");
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
