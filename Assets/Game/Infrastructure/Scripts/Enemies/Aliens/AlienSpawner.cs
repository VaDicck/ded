using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class AlienSpawner<TAlien> : MonoBehaviour where TAlien : AlienBase
{
    [SerializeField] protected List<Ufo> _ufo;
    [SerializeField] protected float _durationSpawn;
    
    protected int _currentSpawn;
    protected int _spawnedCount;
    protected Coroutine _spawnCoroutine;
    
    private bool _canSpawn;
    
    [Inject] protected BedService _bedService;
    [Inject] protected AlienService _alienService;
    
    protected PlaceholderFactory<TAlien> _alienFactory;

    private int _countUfoReady;

    private void Awake()
    {
        foreach (var ufo in _ufo)
        {
            ufo.Arrived += UfoReadySpawn;
        }
        
        _alienService.WaveStart += OnWaveStart;
        _alienService.WaveOver += () => _canSpawn = false;
    }

    private void OnWaveStart()
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
        while (_spawnedCount < GetCountAliens())
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
               && _spawnedCount < GetCountAliens() && _canSpawn;
    }
    
    protected virtual void SpawnAlien()
    {
        if (_currentSpawn >= _ufo.Count)
            _currentSpawn = 0;
        
        TAlien alien = _alienFactory.Create();
        Transform spawnPoint = _ufo[_currentSpawn].transform;
        _ufo[_currentSpawn].Spawn();
        
        alien.SetSpawnPoint(spawnPoint);
        
        _currentSpawn++;
    }

    private void UfoReadySpawn()
    {
        _countUfoReady++;

        if (_countUfoReady == _ufo.Count)
            Spawn();
    }
    
    protected abstract int GetCountAliens();
    
    private void OnDestroy()
    {
        foreach (var ufo in _ufo)
        {
            ufo.Arrived -= UfoReadySpawn;
        }
        
        _alienService.WaveStart -= OnWaveStart;    
    }
}