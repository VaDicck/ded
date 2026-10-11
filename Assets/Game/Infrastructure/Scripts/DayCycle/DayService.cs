using UnityEngine;
using Zenject;

public class DayService
{
    private int _day;
    private DayCycleService _dayCycleService;
    
    public DayService(int startDay, DayCycleService dayCycleService)
    {
        _day = startDay;
        _dayCycleService = dayCycleService;
    }
}