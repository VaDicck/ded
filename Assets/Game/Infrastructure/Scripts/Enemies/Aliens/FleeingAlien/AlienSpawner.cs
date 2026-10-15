using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AlienSpawner : MonoBehaviour
{   
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private int _countAliens;
    [SerializeField] private float _durationSpawn;

    private FleeingAlien.Factory _alienFactory;
    private BedService _bedService;
    private DayCycleService _dayCycleService;
    
    private int _currentSpawn = 0;
    private int _spawnedCount = 0;
    private Coroutine _spawnCoroutine;

    [Inject]
    public void Init(FleeingAlien.Factory alienFactory, BedService bedService, DayCycleService dayCycleService)
    {
        _alienFactory = alienFactory;
        _dayCycleService =  dayCycleService;
        _bedService = bedService;
    }

    private void Awake()
    {
        _dayCycleService.NightStarted += Spawn;
    }

    private void Spawn()
    {
        this.Log("Начинаем спавнить чубрикосиков");

        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        _spawnedCount = 0;
        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (_spawnedCount < _countAliens)
        {
            if (!_bedService.CanAttackBeds)
                break;
            
            if (_spawnedCount > _countAliens)
                break;
            
            SpawnAlien();
            _spawnedCount++;
            
            yield return new WaitForSeconds(_durationSpawn);
        }

        this.Log($"Заспавнено {_spawnedCount} пришельцев");
        _spawnCoroutine = null;
    }

    private void SpawnAlien()
    {
        if (_spawnPoints == null || _spawnPoints.Count == 0)
        {
            this.LogError("Нет точек спавна Fleeing Alien!");
            return;
        }

        if (_currentSpawn >= _spawnPoints.Count-1)
            _currentSpawn = 0;

        FleeingAlien alien = _alienFactory.Create();

        alien.SetSpawnPoint(_spawnPoints[_currentSpawn]);

        _currentSpawn++;
    }

    private void OnDestroy()
    {
        _dayCycleService.NightStarted -= Spawn;

    }
}