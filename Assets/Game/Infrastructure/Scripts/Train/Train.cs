using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Train : MonoBehaviour
{
    [SerializeField] private List<Carriage> _carriages;
    
    private DayCycleService _dayCycleService;
    private QuotaService _quotaService;
    
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

    //TODO rewrite, govno for bedservice
    [ContextMenu("StartNight")]
    public void StartNight()
    {
        _dayCycleService.StartNight();
    }

    private void Arrive()
    {
        Show();
        this.Log("Train arrive");
    }

    private void Leave()
    {
        Hide();
        this.Log("Train Leave");
    }

    private void Hide() => gameObject.SetActive(false);

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
            Hide();
    }
    
    private void OnDestroy()
    {
        _dayCycleService.DayStarted -= Arrive;
        _dayCycleService.NightStarted -= Leave;
    }
}
