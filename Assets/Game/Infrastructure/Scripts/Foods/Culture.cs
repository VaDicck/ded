using System;
using UnityEngine;
using Random = System.Random;

public abstract class Culture : ScriptableObject
{
    public event Action Collected;
    
    [field: SerializeField] public Range DropAmount { get; private set; } = new();
    
    public abstract CultureType Type { get; }
    public int Count => _count;
    
    protected bool IsGrown;
    
    private readonly PlayerCultures _playerCultures;
    private readonly DayCycleService _dayCycleService;
    private int _count;

    protected Culture(PlayerCultures playerCultures, DayCycleService dayCycleService)
    {
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
        if (IsGrown == false)
            return;
        
        Random random = new();
        
        _count = random.Next(DropAmount.Min, DropAmount.Max);

        _playerCultures.AddCultureValue(this);
        Collected?.Invoke();
    }
}