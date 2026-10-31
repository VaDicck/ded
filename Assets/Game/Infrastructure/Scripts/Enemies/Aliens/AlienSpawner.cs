using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class AlienSpawner<TAlien> : MonoBehaviour where TAlien : AlienBase
{
    [SerializeField] protected List<Ufo> _ufo;
    [SerializeField] protected int _countAliens;
    [SerializeField] protected float _durationSpawn;
    
    protected int _currentSpawn = 0;
    protected int _spawnedCount = 0;
    protected Coroutine _spawnCoroutine;

    private bool _canSpawn = false;
    
    protected BedService _bedService;
    protected DayCycleService _dayCycleService;
    
    protected PlaceholderFactory<TAlien> _alienFactory;

    private int _countUfoReady;
    
    [Inject]
    public void Init(BedService bedService, DayCycleService dayCycleService)
    {
        _bedService = bedService;
        _dayCycleService = dayCycleService;
    }

    private void Awake()
    {
        foreach (var ufo in _ufo)
        {
            ufo.Arrived += UfoReadySpawn;
        }
        
        _dayCycleService.NightStarted += OnNightStart;
        _dayCycleService.NightEnded += () => _canSpawn = false;
    }

    private void OnNightStart()
    {
        _canSpawn = false;
        _countUfoReady = 0;
        
        foreach (var ufo in _ufo)
        {
            ufo.gameObject.SetActive(true);
        }
    }
    
    private void Spawn()
    {
        _canSpawn = true;
        
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
        
        _spawnedCount = 0;
        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }
    
    protected virtual IEnumerator SpawnRoutine()
    {
        while (_spawnedCount < _countAliens)
        {
            if (!CanSpawn())
                break;
            
            SpawnAlien();
            _spawnedCount++;
            
            yield return new WaitForSeconds(_durationSpawn);
        }
        
        _spawnCoroutine = null;
    }
    
    protected virtual bool CanSpawn()
    {
        return _bedService.CanAttackBeds.CurrentValue
               && _spawnedCount < _countAliens && _canSpawn;
    }
    
    protected virtual void SpawnAlien()
    {
        if (_ufo.Count == 0)
            return;
        
        if (_currentSpawn >= _ufo.Count)
            _currentSpawn = 0;
        
        TAlien alien = _alienFactory.Create();
        Transform spawnPoint = _ufo[_currentSpawn].transform;
        
        alien.SetSpawnPoint(spawnPoint);
        
        _currentSpawn++;
    }

    private void UfoReadySpawn()
    {
        _countUfoReady++;

        if (_countUfoReady == _ufo.Count)
            Spawn();
    }
    
    private void OnDestroy()
    {
        foreach (var ufo in _ufo)
        {
            ufo.Arrived -= UfoReadySpawn;
        }
        
        _dayCycleService.NightStarted -= OnNightStart;
    }
}