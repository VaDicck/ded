using UnityEngine;
using UnityEngine.AI;

public abstract class Alien<TState> : MonoBehaviour, IDamageable where TState : IState
{ 
    [SerializeField] protected NavMeshAgent _navMeshAgent;
    [SerializeField] protected float _health = 50f;
    
    protected StateMachine<TState> _stateMachine;
    
    protected Transform _bed; 
    protected Transform _spawnPoint; 
    
    public void SetPosition(Vector3 position)
    {
        _navMeshAgent.Warp(position);
    }
    
    public void SetBed(Transform bed)
    {
        _bed = bed;
    }
    
    public void SetSpawnPoint(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
    }
    
    public void TakeDamage(float damage)
    {
        Debug.Log("Попали урон "+ damage);
        _health -= damage;
        
        if(_health <= 0f)
            Die();
    }

    private void Start()
    {
        AddStates();
        
        Subscribe();
    }
    
    private void Update() => _stateMachine.Tick();

    protected abstract void Die();

    protected abstract void Subscribe();
    
    protected abstract void AddStates();
}