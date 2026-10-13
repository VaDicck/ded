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
    
    private void Start()
    {
        _dayCycleService.DayStarted += Arrive;
        _dayCycleService.NightStarted += Leave;
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
            carriage.Show();
        }
    }
    
    private void OnDestroy()
    {
        _dayCycleService.DayStarted -= Arrive;
        _dayCycleService.NightStarted -= Leave;
    }
}
