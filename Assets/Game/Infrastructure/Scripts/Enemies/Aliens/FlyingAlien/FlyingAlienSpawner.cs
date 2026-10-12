using UnityEngine;
using Zenject;

public class FlyingAlienSpawner : MonoBehaviour
{
    private FlyingAlien.Factory _alienFactory;
    
    [SerializeField] private Transform _bed; 
    [SerializeField] private Transform _spawnPoint; 
    [SerializeField] private Transform _player; 
    
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
        
        alien.SetPosition(_spawnPoint.position);
        
        alien.SetBed(_bed);
        alien.SetPlayer(_player);
        alien.SetSpawnPoint(_spawnPoint);
    }
}