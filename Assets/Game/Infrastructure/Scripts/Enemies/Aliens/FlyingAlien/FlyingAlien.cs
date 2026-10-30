using System;
using UnityEngine;
using Zenject;
using R3;

public class FlyingAlien : Alien<IFlyingAlienState>
{
    public event Action PlayerDetected;
    public event Action PlayerLost;
    
    public override AlienType Type => AlienType.Flying;

    [SerializeField] private GameObject _brokenPartsPrefab;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _fireRate = 3f;
    [SerializeField] private float _projectileSpeed = 3f;
    [SerializeField] private float _minShootDistance = 8f;

    [Inject] private BedService _bedService;

    private bool _playerInRange;
    private Transform _playerTransform;
    
    private int _playerLayer;

    private bool _flyToSpawn;

    public class Factory : PlaceholderFactory<FlyingAlien> { }
    
    protected override void AddStates()
    {
        _playerLayer = LayerMask.NameToLayer("Player");
        _animator.SetTrigger("Fly");
        
        _stateMachine = new StateMachine<IFlyingAlienState>(
            new FlyToPointFlyingAlienState(_bedService, _navMeshAgent));

        _stateMachine.AddState<WanderFlyingAlienState>(
            new WanderFlyingAlienState(_navMeshAgent));

        _stateMachine.AddState<AttackFlyingAlienState>(
            new AttackFlyingAlienState(_navMeshAgent , _projectilePrefab, 
                _projectileSpawnPoint, _animator, _fireRate, _projectileSpeed, _minShootDistance));
        
        _stateMachine.AddState<FlyToSpawnFlyingAlienState>(
            new FlyToSpawnFlyingAlienState(_spawnPoint, _navMeshAgent));
        
        _stateMachine.AddState<ChasePlayerFlyingAlienState>(
            new ChasePlayerFlyingAlienState(_navMeshAgent, _minShootDistance));
        
        //TODO maybe in alienBase?
        _bedService.CanAttackBeds.Subscribe(FlyToSpawn);
    }

    protected override void Subscribe()
    {
        ChasePlayerFlyingAlienState chaseState = _stateMachine.GetState<ChasePlayerFlyingAlienState>();
        AttackFlyingAlienState attackState = _stateMachine.GetState<AttackFlyingAlienState>();
        
        _stateMachine.GetState<FlyToPointFlyingAlienState>().Performed += () =>
        {
            _isOnGarden = true;//TODO REWRITE
            _animator.SetTrigger("Fly");
            _stateMachine.SetState<WanderFlyingAlienState>();
        };
        
        _stateMachine.GetState<FlyToSpawnFlyingAlienState>().Performed += () =>
        {
            Destroy(gameObject);
        };
        
        attackState.PlayerTooFar += () =>
        {
            chaseState.SetPlayer(_playerTransform);
            _animator.SetTrigger("Fly");
            _stateMachine.SetState<ChasePlayerFlyingAlienState>();
        };
        
        chaseState.Performed += () =>
        {
            attackState.SetPlayer(_playerTransform);
            _stateMachine.SetState<AttackFlyingAlienState>();
        };
        
        PlayerDetected += () =>
        {
            chaseState.SetPlayer(_playerTransform);
            _animator.SetTrigger("Fly");        
            _stateMachine.SetState<ChasePlayerFlyingAlienState>();
        };

        PlayerLost += () =>
        {
            _animator.SetTrigger("Fly");
            _stateMachine.SetState<WanderFlyingAlienState>();
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_flyToSpawn) return;

        if (other.gameObject.layer != _playerLayer || _playerInRange) return;
        
        _playerInRange = true;
        _playerTransform = other.transform;
        PlayerDetected?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if(_flyToSpawn) return;

        if (other.gameObject.layer != _playerLayer || !_playerInRange) return;
        
        _playerInRange = false;
        _playerTransform = null;
        PlayerLost?.Invoke();
    }

    private void FlyToSpawn(bool value)
    {
        if (value) return;
        
        _animator.SetTrigger("Fly");
        _stateMachine.SetState<FlyToSpawnFlyingAlienState>();
        
        _flyToSpawn = true;
    }

    protected override void Die()
    {
        Instantiate(_brokenPartsPrefab, _projectileSpawnPoint.position, transform.rotation);

        Destroy(gameObject);
    }
}