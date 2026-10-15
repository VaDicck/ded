using System;
using UnityEngine;
using Zenject;

public class DayCycleService : ITickable
{
    public event Action DayStarted;
    public event Action DayEnded;
    public event Action NightStarted;
    public event Action NightEnded;

    private readonly float _dayLength;

    private float _time;
    private bool _isDay;

    public DayCycleService(GameConfig config)
    {
        _dayLength = config.DayLength;
    }

    public void Tick()
    {
        DetermineDay();
    }

    public void StartDay()
    {
        _time = 0;
        _isDay = true;
        DayStarted?.Invoke();
    }
    
    //TODO rewrite
    public void StartNight()
    {
        NightStarted?.Invoke();
    }

    public void EndNight()
    {
        NightEnded?.Invoke();
    }

    private void DetermineDay()
    {
        if (_isDay == false)
            return;

        _time += Time.deltaTime;

        if (_time >= _dayLength)
        {
            _isDay = false;
            DayEnded?.Invoke();
            NightStarted?.Invoke();
        }
    }

}
