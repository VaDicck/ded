using System;
using System.Collections;
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
    [SerializeField] private Renderer _meshRenderer;  
    [SerializeField] private Material _hitMaterial;
    [SerializeField] private float _durationHit;
    
    [SerializeField] private GameObject _sfxSplashAlien;
    
    [Header("Audio")]
    [SerializeField] protected AudioSource _audio;
    
    protected Transform _spawnPoint;
    
    private AlienService _alienService;
    
    private bool _isDead;

    protected ReactiveProperty<bool> _isOnGarden = new ReactiveProperty<bool>(); //TODO rewrite
    
    private PlaceholderFactory<AlienBroken> _brokenFactory;
    
    private Coroutine _hitEffectCoroutine;
    private float _originalAnimatorSpeed;
    private Material _originalMaterial;
    private float _originalAgentSpeed;

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
    
    public void TakeDamage(float damage, Vector3 hitPoint, AudioClip audioClip = null)
    {
        if(_isDead) return;
        
        _health -= damage;

        if(audioClip != null)
            _audio.PlayOneShot(audioClip);
        
        SpawnSfx(hitPoint);
        
        ApplyHitEffect();
        
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

    private void ApplyHitEffect()
    {
        if (_hitEffectCoroutine != null)
            StopCoroutine(_hitEffectCoroutine);
        else
        {
            _originalAnimatorSpeed = _animator.speed;
            _originalMaterial = _meshRenderer.material;
            _originalAgentSpeed = _navMeshAgent.speed;  
            
            _animator.speed = 0f;
            _navMeshAgent.speed = 0f;
            _navMeshAgent.velocity = Vector3.zero; 
            _meshRenderer.material = _hitMaterial;
        }

        _hitEffectCoroutine = StartCoroutine(RestoreAfterDelay());
    }

    private IEnumerator RestoreAfterDelay()
    {
        yield return new WaitForSeconds(_durationHit);
        _animator.speed = _originalAnimatorSpeed;
        _meshRenderer.material = _originalMaterial;
        _navMeshAgent.speed = _originalAgentSpeed;   
        _hitEffectCoroutine = null;
    }
    
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
        if (_hitEffectCoroutine != null)
            StopCoroutine(_hitEffectCoroutine);
        Death?.Invoke(this);
    }
}
