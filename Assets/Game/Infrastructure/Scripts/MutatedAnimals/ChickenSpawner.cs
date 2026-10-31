using UnityEngine;

public class ChickenSpawner
{
    private Chicken.Factory _chickenFactory;
    private Vector3 _spawnPoint;
    
    public ChickenSpawner(Vector3 spawnPoint, Chicken.Factory factory)
    {
        _chickenFactory = factory;
        _spawnPoint = spawnPoint;
    }
    
    public void Spawn()
    {
        var chicken = _chickenFactory.Create();
        chicken.WarpTo(_spawnPoint);
    }
}