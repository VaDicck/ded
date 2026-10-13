using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Train : MonoBehaviour
{
    [SerializeField] private List<Carriage> _carriages;
    
    private DayCycleService _dayCycleService;
    private QuotaService _quotaService;
    
    [Inject]
    private void Init(DayCycleService _dayCycle, QuotaService quotaService)
    {
        _dayCycleService =  _dayCycle;
        _quotaService = quotaService;
    }

    private void Awake()
    {
        UpdateCarriage();
        
        _dayCycleService.DayStarted += Arrive;
        _dayCycleService.NightStarted += Leave;
        _quotaService.QuotaExpired += UpdateCarriage;
    }

    private void Arrive()
    {
        Debug.Log("<color=red>Train arrived!</color>");
    }

    private void Leave()
    {
        Debug.Log("<color=red>Train left</color>");
    }
    
    private void UpdateCarriage()
    {
        Dictionary<Quota.Element, int> quotaMappings = new Dictionary<Quota.Element, int>
        {
            { Quota.Element.Main, 0 },
            { Quota.Element.Sub1, 1 },
            { Quota.Element.Sub2, 2 }
        };
        
        foreach (var mapping in quotaMappings)
        {
            bool hasQuota = _quotaService.CurrentQuota.TryGet(mapping.Key, out var quota);
            _carriages[mapping.Value].gameObject.SetActive(hasQuota);
            _carriages[mapping.Value].gameObject.GetComponent<Carriage>().SetQuotaElement(quota);
        }
    }

    private void OnDestroy()
    {
        _dayCycleService.DayStarted -= Arrive;
        _dayCycleService.NightStarted -= Leave;
        _quotaService.QuotaExpired -= UpdateCarriage;
    }
}
