using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class QuotaService
{
    public event Action QuotaReached;
    public event Action<Quota.Element, CultureType, int> CultureAmountChanged;

    public Quota CurrentQuota => _currentQuota;

    private readonly DayService _dayService;

    private Quota _currentQuota;
    private GameConfig _gameConfig;
    private int _currentQuotaNumber;
    
    public QuotaService(int quotaNumber, GameConfig config)
    {
        _gameConfig = config;
        _currentQuotaNumber = quotaNumber;
        
        SetCurrentQuota(_currentQuotaNumber);
        QuotaReached += OnQuotaReached;
    }

    private void OnQuotaReached()
    {
        _currentQuotaNumber++;
        SetCurrentQuota(_currentQuotaNumber);
    }

    private void OnQuotaElementDone()
    {
        if((CurrentQuota.Main.IsEmpty || CurrentQuota.Main.IsReached)
           && (CurrentQuota.Sub1.IsEmpty || CurrentQuota.Sub1.IsReached)
           && (CurrentQuota.Sub2.IsEmpty ||  CurrentQuota.Sub2.IsReached))
            QuotaReached?.Invoke();
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
        if (_gameConfig.Quotas.Quotas.Count < quotaNumber + 1)
        {
            this.LogError($"Quota for day {quotaNumber} not found");
            _currentQuota = null;
            return;
        }
        
        _currentQuota = Object.Instantiate(_gameConfig.Quotas.Quotas[quotaNumber]);
        
        _currentQuota.Main.Reached += OnQuotaElementDone;
        _currentQuota.Sub1.Reached += OnQuotaElementDone;
        _currentQuota.Sub2.Reached += OnQuotaElementDone;
        
        this.Log($"Quota \"{quotaNumber}\": {_currentQuota}");
    }
}