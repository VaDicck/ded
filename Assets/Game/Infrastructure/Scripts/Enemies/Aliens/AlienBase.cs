using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public abstract class AlienBase :  MonoBehaviour, IDamageable
{
    public event Action<AlienBase> Death;
    
    [SerializeField] protected NavMeshAgent _navMeshAgent;
    [SerializeField] protected float _health = 50f;
    [SerializeField] protected Animator _animator;
    
    protected Transform _bed; 
    protected Transform _spawnPoint;
    
    private AlienService _alienService;
    
    private bool _isDead;

    [Inject]
    private void Init(AlienService alienService)
    {
        _alienService = alienService;
    }
    
    private void Awake()
    {
        _alienService.AddAlien(this);
        _isDead = false;
    }
    
    public void SetSpawnPoint(Transform spawnPoint)
    {
        _navMeshAgent.Warp(spawnPoint.position);
        _spawnPoint = spawnPoint;
    }
    
    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0f && !_isDead)
        {
            _isDead = true;
            Die();
        }
    }
    
    protected abstract void Die();

    private void OnDestroy()
    {
        Death?.Invoke(this);
    }
}
