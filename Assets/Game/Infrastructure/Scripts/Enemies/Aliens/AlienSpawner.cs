using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class AlienSpawner<TAlien> : MonoBehaviour where TAlien : IAlien
{
    [SerializeField] protected List<Transform> _spawnPoints;
    [SerializeField] protected int _countAliens;
    [SerializeField] protected float _durationSpawn;
    
    protected int _currentSpawn = 0;
    protected int _spawnedCount = 0;
    protected Coroutine _spawnCoroutine;

    private bool _canSpawn = false;
    
    protected BedService _bedService;
    protected DayCycleService _dayCycleService;
    
    protected PlaceholderFactory<TAlien> _alienFactory;
    
    [Inject]
    public void Init(BedService bedService, DayCycleService dayCycleService)
    {
        _bedService = bedService;
        _dayCycleService = dayCycleService;
        OnInit();
    }
    
    //for fabrics
    protected virtual void OnInit() { }
    
    private void Awake()
    {
        _dayCycleService.NightStarted += Spawn;
        _dayCycleService.NightEnded += () => _canSpawn = false;
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
        return _bedService.CanAttackBeds && _spawnedCount < _countAliens && _canSpawn;
    }
    
    protected virtual void SpawnAlien()
    {
        if (_spawnPoints == null || _spawnPoints.Count == 0)
            return;
        
        if (_currentSpawn >= _spawnPoints.Count)
            _currentSpawn = 0;
        
        TAlien alien = _alienFactory.Create();
        Transform spawnPoint = _spawnPoints[_currentSpawn];
        
        alien.SetSpawnPoint(spawnPoint);
        
        _currentSpawn++;
    }
    
    private void OnDestroy()
    {
        _dayCycleService.NightStarted -= Spawn;
    }
}