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
    private int _currentSpawn = 0;
    private BedService _bedService;
    private int _spawnedCount = 0;
    private Coroutine _spawnCoroutine;

    private int _countPlantedBed=0;
    private int _currentCountPlantedBed=0;

    [Inject]
    public void Init(FleeingAlien.Factory alienFactory, BedService bedService)
    {
        _alienFactory = alienFactory;
        _bedService = bedService;
    }

    [ContextMenu("Spawning")]
    public void Spawn()
    {
        this.Log("Начинаем спавнить чубрикосиков");
        _countPlantedBed = _bedService.GetBedsByState<PlantedBedState>().Count;
        _currentCountPlantedBed = _countPlantedBed;

        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        _spawnedCount = 0;
        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (_spawnedCount < _countAliens)
        {
            SpawnAlien();
            _spawnedCount++;

            if (_spawnedCount >= _countAliens)
                break;

            _currentCountPlantedBed = _bedService.GetBedsByState<PlantedBedState>().Count;

            if (((float)_currentCountPlantedBed / (float)_countPlantedBed) <= _bedService.ProcentSaveBed)
                break;

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
}