using System;
using System.Collections.Generic;
using UnityEngine;

public class AlienService
{
    public event Action WaveStart;
    public event Action WaveOver;
    
    public bool WaveActive => _waveActive;

    public int CountFleeingAlienInWave => _countFleeingAlienInWave;
    public int CountFlyingAlienInWave => _countFlyingAlienInWave;
    public int CountAlienInWave => CountFleeingAlienInWave+CountFlyingAlienInWave;
    
    public bool AliensEmpty => _aliens.Count == 0;
    public HashSet<AlienBase> Aliens => _aliens;

    private int _currentCountAlienInWave = 0;

    private int _countFleeingAlienInWave;
    private int _countFlyingAlienInWave;
    
    private int _countFleeingAlienOneBed = 2;
    private int _countFlyingAlienOneBed = 1;
    
    private HashSet<AlienBase> _aliens  = new HashSet<AlienBase>();
    
    private readonly DayCycleService _dayCycleService;
    private readonly BedService _bedService;
    private readonly DayService _dayService;

    private bool _waveActive;

    public AlienService(DayCycleService dayCycleService, BedService bedService, DayService dayService)
    {
        _dayCycleService = dayCycleService;
        _bedService = bedService;
        _dayService = dayService;
        
        _dayCycleService.NightStarted += OnNightStart;
    }
    
    public void AddAlien(AlienBase alien)
    {
        if (!_aliens.Add(alien)) return;
        
        _currentCountAlienInWave++;
        alien.Death += RemoveAlien;
    }

    private void RemoveAlien(AlienBase alien)
    {
        alien.Death -= RemoveAlien;
        _aliens.Remove(alien);

        if (_bedService.CanAttackBeds.CurrentValue)
        {
            if (_currentCountAlienInWave == CountAlienInWave)
            {
                if(AliensEmpty)
                    StopWave();
            }
            
            return;
        }
        
        if(AliensEmpty)
            StopWave();
    }

    private void OnNightStart()
    {
        if (_bedService.CanAttackBeds.CurrentValue)
        {
            StartWave();
        }
    }

    private void StartWave()
    {
        int countPlantedBed = _bedService.GetBedsByState<PlantedBedState>().Count;

        if (_dayService.Day == 1)
        {
            _countFleeingAlienInWave = 2;
            _countFlyingAlienInWave = 0;
        }
        if (_dayService.Day == 2)
        {
            _countFleeingAlienInWave = 4;
            _countFlyingAlienInWave = 0;
        }
        if (_dayService.Day < 5)
        {
            _countFleeingAlienInWave = CalculateAliens(countPlantedBed);
            _countFlyingAlienInWave = 0;
        }
        else
        {
            _countFleeingAlienInWave = CalculateAliens(countPlantedBed);
            _countFlyingAlienInWave = _countFleeingAlienInWave / 3;
        }
        
        _waveActive = true;
        WaveStart?.Invoke();
    }

    private void StopWave()
    {
        _currentCountAlienInWave = 0;
        _waveActive = false;
        WaveOver?.Invoke();
    }
    
    private int CalculateAliens(int beds, int helpers = 0, float baseCoefficient = 1.5f, float helperExponent = 0.5f)
    {
        if (beds <= 0) return 0;
        
        int defenders = 1 + helpers;
        float multiplier = Mathf.Pow(defenders, helperExponent);
        
        float rawValue = baseCoefficient * beds * multiplier;
        
        return Mathf.RoundToInt(rawValue);
    }
}
