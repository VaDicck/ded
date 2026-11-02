using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using R3;

public abstract class AlienBase :  MonoBehaviour, IDamageable
{
    public event Action<AlienBase> Death;
    
    public ReadOnlyReactiveProperty<bool> IsOnGarden => _isOnGarden;

    public abstract AlienType Type { get; }

    [SerializeField] protected NavMeshAgent _navMeshAgent;
    [SerializeField] protected float _health = 50f;
    [SerializeField] protected Animator _animator;
    
    [SerializeField] private GameObject _sfxSplashAlien;
    
    [Header("Audio")]
    [SerializeField] protected AudioSource _audio;
    
    protected Transform _spawnPoint;
    
    private AlienService _alienService;
    
    private bool _isDead;

    protected ReactiveProperty<bool> _isOnGarden = new ReactiveProperty<bool>(); //TODO rewrite
    
    private PlaceholderFactory<AlienBroken> _brokenFactory;

    [Inject]
    private void Init(AlienService alienService, AlienBroken.Factory alienFactory)
    {
        _alienService = alienService;
        _brokenFactory = alienFactory;
    }
    
    private void Awake()
    {
        _alienService.AddAlien(this);
        _isDead = false;
    }
    
    public void SetSpawnPoint(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        
        _navMeshAgent.Warp(spawnPoint.position);
        _navMeshAgent.enabled = true;
        
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
            Destroy(effect, 2f);
        }
    }

    protected void Die()
    {
        SpawnBroken();

        OnDie();
        
        Destroy(gameObject);
    }

    protected virtual void OnDie() { }

    private void SpawnBroken()
    {
        AlienBroken broken = _brokenFactory.Create();
    
        broken.transform.SetPositionAndRotation(
            transform.position, 
            transform.rotation
        );
    }

    private void OnDestroy()
    {
        Death?.Invoke(this);
    }
}
