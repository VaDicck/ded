using System;
using R3;
using UnityEngine;

public abstract class Culture: IDisposable
{
    public event Action Collected;
    public event Action<bool> Watered;
    public event Action Grown;

    public Range DropAmount => _config.DropAmount;
    public CultureConfig CultureConfig => _config;
    public ReadOnlyReactiveProperty<bool> IsGrown => _isGrown;
    public ReadOnlyReactiveProperty<bool> IsPoured => _isPoured;
    
    public abstract CultureType Type { get; }
    public int Count => _count;
    
    protected readonly ReactiveProperty<bool> _isGrown = new(false);
    
    protected readonly PlayerCultures _playerCultures;
    private readonly DayCycleService _dayCycleService;
    protected readonly CultureConfig _config;
    
    protected int _count;

    protected float _timerCollected = 10f;
    private float _currentTimerCollected;
    private int _countEnemyHitting = 0;
    
    protected ReactiveProperty<bool> _isPoured = new(false);
    
    
    protected Transform _bedTransform;
    

    protected Culture(CultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
    {
        _config = config;
        _playerCultures = playerCultures;
        _dayCycleService = dayCycleService;
        _dayCycleService.DayStarted += OnDayStarted;

        _currentTimerCollected = _timerCollected;
    }

    public void SetBedTransform(Transform bedTransform)
    {
        _bedTransform = bedTransform;
        OnSetTransform();
    }
    
    //TODO hz
    protected abstract void OnSetTransform();
    
    public abstract ToolType UseTool();
    
    public abstract bool UseTool(ToolType toolType);
    
    public abstract bool IsInteract();

    public abstract bool IsUsable();

    public abstract void Interact();

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
    
    protected virtual void OnDayStarted()
    {
        if (!_isPoured.Value)
            return;

        _isGrown.Value = true;
    }
    
    protected void RaiseCollected() =>  Collected?.Invoke();
    protected void RaiseWatered(bool value) =>  Watered?.Invoke(value);
    protected void RaiseGrown() =>  Grown?.Invoke();
    
    public void Dispose()
    {
        _dayCycleService.DayStarted -= OnDayStarted;
    }
}