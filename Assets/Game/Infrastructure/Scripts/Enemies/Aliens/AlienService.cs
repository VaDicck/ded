using System;
using System.Collections.Generic;

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

    private bool _waveActive;

    public AlienService(DayCycleService dayCycleService, BedService bedService)
    {
        _dayCycleService = dayCycleService;
        _bedService = bedService;
        
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

        _countFleeingAlienInWave = countPlantedBed * _countFleeingAlienOneBed;
        _countFlyingAlienInWave = countPlantedBed * _countFlyingAlienOneBed;
        
        _waveActive = true;
        WaveStart?.Invoke();
    }

    private void StopWave()
    {
        _currentCountAlienInWave = 0;
        _waveActive = false;
        WaveOver?.Invoke();
    }
}
