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
    
    private PlayerResources _playerIngredients;
    
    private GameConfig _gameConfig;
    private int _currentQuotaNumber;

    public QuotaService(int quotaNumber, PlayerResources playerIngredients, GameConfig config)
    {
        _currentQuotaNumber = quotaNumber;
        _playerIngredients = playerIngredients;
        _gameConfig = config;

        SetCurrentQuota(_currentQuotaNumber);
        QuotaReached += OnQuotaReached;
    }

    private void OnQuotaReached()
    {
        ApplyReward();
        SetCurrentQuota(++_currentQuotaNumber);
    }

    private void ApplyReward()
    {
        if (_currentQuota.Main.IsEmpty == false)
            _currentQuota.Main.ApplyReward(_playerIngredients);
        
        if (_currentQuota.Sub1.IsEmpty == false)
            _currentQuota.Sub1.ApplyReward(_playerIngredients);
        
        if (_currentQuota.Sub2.IsEmpty == false)
            _currentQuota.Sub2.ApplyReward(_playerIngredients);
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