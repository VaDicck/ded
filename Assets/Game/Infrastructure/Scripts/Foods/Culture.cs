using System;
using UnityEngine;
using Random = System.Random;

public abstract class Culture
{
    public event Action Collected;
    
    public Range DropAmount => _config.DropAmount;
    public GameObject Prefab => _config.Prefab;
    
    public abstract CultureType Type { get; }
    public int Count => _count;
    
    [SerializeField] protected bool IsGrown = true;
    
    private readonly PlayerCultures _playerCultures;
    private readonly DayCycleService _dayCycleService;
    private readonly CultureConfig _config;
    
    private int _count;

    private float _timerCollected = 30f;

    protected Culture(CultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
    {
        _config = config;
        _playerCultures = playerCultures;
        _dayCycleService = dayCycleService;
        _dayCycleService.DayStarted += OnDayStarted;
    }

    protected virtual void OnDayStarted()
    {
        IsGrown = true;
    }
    
    public void Collect()
    {
        Debug.Log(IsGrown);
        
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