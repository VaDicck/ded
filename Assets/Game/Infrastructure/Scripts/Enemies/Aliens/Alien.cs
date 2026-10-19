using UnityEngine;
using UnityEngine.AI;

public abstract class Alien<TState> : MonoBehaviour, IDamageable, IAlien where TState : IState
{ 
    [SerializeField] protected NavMeshAgent _navMeshAgent;
    [SerializeField] protected float _health = 50f;
    [SerializeField] protected Animator _animator;
    
    protected StateMachine<TState> _stateMachine;
    
    protected Transform _bed; 
    protected Transform _spawnPoint; 
    
    public void SetSpawnPoint(Transform spawnPoint)
    {
        _navMeshAgent.Warp(spawnPoint.position);
        _spawnPoint = spawnPoint;
    }
    
    public void TakeDamage(float damage)
    {
        this.Log($"Попали, урон {damage}");
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