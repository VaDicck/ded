using System;
using UnityEngine;

public class ChickenSpawner
{
    public event Action Spawned;
    
    private Chicken.Factory _chickenFactory;
    private Transform _spawnPoint;
    
    public ChickenSpawner(Transform spawnPoint, Chicken.Factory factory)
    {
        _chickenFactory = factory;
        _spawnPoint = spawnPoint;
    }
    
    public void Spawn()
    {
        var chicken = _chickenFactory.Create();
        chicken.WarpTo(_spawnPoint.position);
        chicken.SetSpawnPoint(_spawnPoint);
        Spawned?.Invoke();
    }
}