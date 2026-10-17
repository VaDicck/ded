using System;
using UnityEngine;
using Random = System.Random;

public abstract class Culture
{
    public event Action Collected;

    public Range DropAmount => _config.DropAmount;
    public CultureConfig CultureConfig => _config;
    public bool IsGrown { get; protected set; }
    
    public abstract CultureType Type { get; }
    public int Count => _count;
    
    
    private readonly PlayerCultures _playerCultures;
    private readonly DayCycleService _dayCycleService;
    private readonly CultureConfig _config;
    
    private int _count;

    private float _timerCollected = 6f;

    private bool _isPoured = false;

    protected Culture(CultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
    {
        _config = config;
        _playerCultures = playerCultures;
        _dayCycleService = dayCycleService;
        _dayCycleService.DayStarted += OnDayStarted;
    }

    protected virtual void OnDayStarted()
    {
        if (!_isPoured)
            return;

        IsGrown = true;
    }
    
    public void UseTool(ToolType toolType)
    {
        switch (toolType)
        {
            case ToolType.Bailer:
                _isPoured = true;
                break;
        }
    }

    public void Collect()
    {
        if (_isPoured == false)
            return;

        if (IsGrown == false)
            return;
        
        this.Log($"{Type} collected");
        
        Random random = new();
        
        _count = random.Next(DropAmount.Min, DropAmount.Max);

        _playerCultures.AddCultureValue(this);
        Collected?.Invoke();
    }

    public void Hit(float value)
    {
        _timerCollected -= value;

        if (_timerCollected < 0)
        {
            Collected?.Invoke();
        }
    }
}