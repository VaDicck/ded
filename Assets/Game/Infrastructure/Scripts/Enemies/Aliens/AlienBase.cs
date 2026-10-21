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
    
    [SerializeField] private GameObject _sfxSplashAlien;
    
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
    
    public void TakeDamage(float damage, Vector3 hitPoint)
    {
        if(_isDead) return;
        
        _health -= damage;

        SpawnSfx(hitPoint);

        if (_health > 0f) return;
        
        _isDead = true;
        Die();
    }

    private void SpawnSfx(Vector3 spawnPoint)
    {
        if (_sfxSplashAlien != null)
        {
            GameObject effect = Instantiate(_sfxSplashAlien, spawnPoint, Quaternion.identity);
            effect.transform.SetParent(transform);
        }
    }
    
    protected abstract void Die();

    private void OnDestroy()
    {
        Death?.Invoke(this);
    }
}
