using System;
using UnityEngine;

public class DayService
{
    public event Action<int> NewDayReached;
    public int Day { get; private set; }

    private DayCycleService _dayCycleService;
    
    public DayService(int startDay, DayCycleService dayCycleService)
    {
        Day = startDay;
        _dayCycleService = dayCycleService;
        _dayCycleService.NightEnded += OnDayEnded;
    }

    private void OnDayEnded()
    {
        Day++;
        
        Debug.Log($"<color=yellow> Day: {Day} reached");
        
        NewDayReached?.Invoke(Day);
    }
}