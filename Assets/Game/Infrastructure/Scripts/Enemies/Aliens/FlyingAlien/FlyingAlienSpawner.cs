using UnityEngine;
using Zenject;

public class FlyingAlienSpawner : MonoBehaviour
{
    private FlyingAlien.Factory _alienFactory;
    
    [Inject]
    public void Init(FlyingAlien.Factory alienFactory)
    {
        _alienFactory = alienFactory;
    }
    
    private void Start()
    {
        InvokeRepeating(nameof(SpawnAlien), 5f, 30f); 
    }
    
    private void SpawnAlien()
    {
        FlyingAlien alien = _alienFactory.Create();
        
        alien.SetSpawnPoint(transform);
    }
}