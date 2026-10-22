using System;
using R3;
using UnityEngine;
using Zenject;
using Random = System.Random;

public abstract class Culture: IDisposable
{
    public event Action Collected;

    public Range DropAmount => _config.DropAmount;
    public CultureConfig CultureConfig => _config;
    public ReadOnlyReactiveProperty<bool> IsGrown => _isGrown;
    public ReadOnlyReactiveProperty<bool> IsPoured => _isPoured;
    
    public abstract CultureType Type { get; }
    public int Count => _count;
    
    private  readonly ReactiveProperty<bool> _isGrown = new(false);
    
    private readonly PlayerCultures _playerCultures;
    private readonly DayCycleService _dayCycleService;
    private readonly CultureConfig _config;
    
    private int _count;

    protected float _timerCollected = 10f;
    private float _currentTimerCollected;
    private int _countEnemyHitting = 0;

    private ReactiveProperty<bool> _isPoured = new(false);

    protected Culture(CultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
    {
        _config = config;
        _playerCultures = playerCultures;
        _dayCycleService = dayCycleService;
        _dayCycleService.DayStarted += OnDayStarted;

        _currentTimerCollected = _timerCollected;
    }

    protected virtual void OnDayStarted()
    {
        if (!_isPoured.Value)
            return;

        _isGrown.Value = true;
    }
    
    public bool UseTool(ToolType toolType)
    {
        switch (toolType)
        {
            case ToolType.Bailer:
                _isPoured.Value = true;
                return true;
        }

        return false;
    }

    public void Collect()
    {
        if (_isPoured.Value == false)
            return;

        if (_isGrown.Value == false)
            return;
        
        this.Log($"{Type} collected");
        
        Random random = new();
        
        _count = random.Next(DropAmount.Min, DropAmount.Max);

        _playerCultures.AddCultureValue(this);
        Collected?.Invoke();
    }

    public void StartHitting()
    {
        _countEnemyHitting++;
    }
    
    public void Hit(float value)
    {
        _currentTimerCollected -= value;

        if (_currentTimerCollected < 0)
        {
            Collected?.Invoke();
            _countEnemyHitting = 0;
        }
    }
    
    public void StopHitting()
    {
        _countEnemyHitting--;
        if (_countEnemyHitting == 0)
        {
            _currentTimerCollected = _timerCollected;
        }
    }

    public void Dispose()
    {
        _dayCycleService.DayStarted -= OnDayStarted;
    }
}