using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class QuotaService
{
    public event Action QuotaExpired;
    public event Action<Quota.Element, CultureType, int> CultureAmountChanged;

    public Quota CurrentQuota => _currentQuota;

    private readonly DayService _dayService;
    private readonly GameConfig _gameConfig;

    private Quota _currentQuota;
    private int _quotaNumber;
    private int _daysPassed;
    
    public QuotaService(int quotaNumber, int daysPassed, DayService dayService, GameConfig config)
    {
        _quotaNumber = quotaNumber;
        _daysPassed = daysPassed;
        
        _dayService = dayService;
        _gameConfig = config;

        _dayService.NewDayReached += _ => OnNewDayReached();
        
        SetCurrentQuota(_quotaNumber);
    }

    private void OnQuotaElementDone()
    {
        
    }
    
    private void OnNewDayReached()
    {
        _daysPassed++;

        if (_daysPassed >= _gameConfig.QuotaExpireDays)
            QuotaExpired?.Invoke();
    }

    public int AddCulture(Quota.Element element, CultureType cultureType, int value)
    {
        if (_currentQuota.TryGet(element, out QuotaElement quota))
        {
            int filledValue = quota.Fill(cultureType, value);

            if (filledValue > 0)
            {
                CultureAmountChanged?.Invoke(element, cultureType, filledValue);
            }
            
            return filledValue;
        }

        return 0;
    }

    private void SetCurrentQuota(int quotaNumber)
    {
        if (_gameConfig.Quotas.Quotas.Count < quotaNumber)
        {
            Debug.LogError($"Quota for day {quotaNumber} not found");
            _currentQuota = null;
            return;
        }
        
        _currentQuota = Object.Instantiate(_gameConfig.Quotas.Quotas[quotaNumber]);
        
        Debug.Log($"Quota \"{quotaNumber}\": {_currentQuota}");
    }
}