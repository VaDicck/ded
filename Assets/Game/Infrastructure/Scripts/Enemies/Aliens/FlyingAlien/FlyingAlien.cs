using System;
using UnityEngine;
using Zenject;
using R3;

public class FlyingAlien : Alien<IFlyingAlienState>
{
    public event Action PlayerDetected;
    public event Action PlayerLost;

    [SerializeField] private GameObject _brokenPartsPrefab;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _fireRate = 3f;
    [SerializeField] private float _projectileSpeed = 3f;

    public override AlienType Type => AlienType.Flying;

    [Inject] private BedService _bedService;

    private bool _playerInRange;
    private Transform _playerTransform;
    
    private int _playerLayer;

    private bool _flySpawn;

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
                _projectileSpawnPoint, _animator, _fireRate, _projectileSpeed));
        
        _stateMachine.AddState<FlyToSpawnFlyingAlienState>(
            new FlyToSpawnFlyingAlienState(_spawnPoint, _navMeshAgent));
        
        //TODO maybe in alienBase?
        _bedService.CanAttackBeds.Subscribe(FlyToSpawn);
    }

    protected override void Subscribe()
    {
        _stateMachine.GetState<FlyToPointFlyingAlienState>().Performed += () =>
        {
            _animator.SetTrigger("Fly");
            _stateMachine.SetState<WanderFlyingAlienState>();
        };

        PlayerDetected += () =>
        {
            AttackFlyingAlienState attackState = _stateMachine.GetState<AttackFlyingAlienState>();
            attackState.SetPlayer(_playerTransform);
            _animator.SetTrigger("Attack");
            _stateMachine.SetState<AttackFlyingAlienState>();
        };

        PlayerLost += () =>
        {
            if (_stateMachine.CurrentState is AttackFlyingAlienState)
            {
                _animator.SetTrigger("Fly");
                _stateMachine.SetState<WanderFlyingAlienState>();
            }
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_flySpawn) return;

        if (other.gameObject.layer != _playerLayer || _playerInRange) return;
        
        _playerInRange = true;
        _playerTransform = other.transform;
        PlayerDetected?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if(_flySpawn) return;

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
        
        _flySpawn = true;
            
        _stateMachine.GetState<FlyToPointFlyingAlienState>().Performed += () =>
        {
            Destroy(gameObject);
        };
    }

    protected override void Die()
    {
        Instantiate(_brokenPartsPrefab, _projectileSpawnPoint.position, transform.rotation);

        Destroy(gameObject);
    }
}