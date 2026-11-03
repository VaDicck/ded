using System;
using R3;
using UnityEngine;

public abstract class Culture: IDisposable
{
    public event Action Collected;
    public event Action<bool> Watered;
    public event Action Grown;
    
    public CultureConfig CultureConfig => _config;
    public ReadOnlyReactiveProperty<bool> IsGrown => _isGrown;
    public ReadOnlyReactiveProperty<bool> IsPoured => _isPoured;
    
    public abstract CultureType Type { get; }
    public int Count => _count;
    
    protected readonly ReactiveProperty<bool> _isGrown = new(false);
    
    protected readonly PlayerCultures _playerCultures;
    protected readonly DayCycleService _dayCycleService;
    protected readonly CultureConfig _config;
    
    protected int _count;

    protected float _timerDigOut = 10f;
    private float _currentTimerCollected;
    private int _countEnemyHitting = 0;
    
    protected ReactiveProperty<bool> _isPoured = new(false);
    
    
    protected Transform _bedTransform;
    
    protected PlantedBedState _plantedBedState;
    

    protected Culture(CultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
    {
        //TODO UNSIBSCRIBE
        IsPoured.Subscribe(RaiseWatered);
        
        _config = config;
        _playerCultures = playerCultures;
        _dayCycleService = dayCycleService;
        _dayCycleService.DayStarted += OnDayStarted;

        _currentTimerCollected = _timerDigOut;
    }

    public void SetBedTransform(Transform bedTransform)
    {
        _bedTransform = bedTransform;
        OnSetTransform();
    }
    
    public void SetPlantedBedState(PlantedBedState plantedBedState) => _plantedBedState = plantedBedState;
    
    protected abstract void OnSetTransform();
    
    public abstract ToolType UseTool();

    public bool UseTool(ToolType toolType)
    {
        if (toolType == ToolType.Shovel)
        {
            DigOut();
            RaiseCollected();
            _countEnemyHitting = 0;
            return true;
        }

        if (toolType == ToolType.Bailer)
        {
            TryPour();
            return true;
        }
        
        return OnUseTool(toolType);
    }

    protected abstract bool OnUseTool(ToolType toolType);
    
    public abstract bool IsInteract();

    public abstract bool IsUsable();

    public abstract void Interact();

    public void StartHitting()
    {
        if (_countEnemyHitting == 0)
        {
            _currentTimerCollected = _timerDigOut;
        }
        _countEnemyHitting++;
    }
    
    public void Hit(float value)
    {
        _currentTimerCollected -= value;

        if (_currentTimerCollected < 0)
        {
            DigOut();
            RaiseCollected();
            _countEnemyHitting = 0;
        }
    }
    
    public void StopHitting()
    {
        _countEnemyHitting--;
        if (_countEnemyHitting == 0)
        {
            _currentTimerCollected = _timerDigOut;
        }
    }
    
    protected bool TryPour()
    {
        if(_isPoured.Value) return false;
        _isPoured.Value = true;
        return true;
    }
    
    protected virtual void OnDayStarted()
    {
        if (!_isPoured.Value)
            return;

        _isGrown.Value = true;
    }

    protected virtual void DigOut() { }
    
    protected void RaiseCollected() =>  Collected?.Invoke();
    protected void RaiseWatered(bool value) =>  Watered?.Invoke(value);
    protected void RaiseGrown() =>  Grown?.Invoke();
    
    public virtual void Dispose()
    {
        _dayCycleService.DayStarted -= OnDayStarted;
    }
}