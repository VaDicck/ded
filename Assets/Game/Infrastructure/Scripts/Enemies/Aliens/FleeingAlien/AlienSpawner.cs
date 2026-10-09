using UnityEngine;
using Zenject;

public class AlienSpawner : MonoBehaviour
{
    private FleeingAlien.Factory _alienFactory;
    
    [SerializeField] private Transform _bed; 
    [SerializeField] private Transform _spawnPoint; 
    
    [Inject]
    public void Init(FleeingAlien.Factory alienFactory)
    {
        _alienFactory = alienFactory;
    }
    
    private void Start()
    {
        InvokeRepeating(nameof(SpawnAlien), 5f, 3f); 
    }
    
    private void SpawnAlien()
    {
        FleeingAlien alien = _alienFactory.Create();
        
        alien.transform.position = _spawnPoint.position;
        
        alien.SetBed(_bed);
        alien.SetSpawnPoint(_spawnPoint);
        
        Debug.Log("Пришелец заспавнен!");
    }
}