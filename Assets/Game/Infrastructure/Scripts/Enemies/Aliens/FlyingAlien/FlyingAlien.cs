using System;
using UnityEngine;
using Zenject;

public class FlyingAlien : Alien<IFlyingAlienState>
{
    [SerializeField] private GameObject _brokenPartsPrefab;

    public override AlienType Type => AlienType.Flying;

    [Inject] private BedService _bedService;

    private bool _playerInRange;
    private Transform _playerTransform;

    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _fireRate = 3f;
    [SerializeField] private float _projectileSpeed = 30f;

    public event Action PlayerDetected;
    public event Action PlayerLost;

    public class Factory : PlaceholderFactory<FlyingAlien> { }

    protected override void AddStates()
    {
        _animator.SetTrigger("Fly");
        _stateMachine = new StateMachine<IFlyingAlienState>(
            new FlyToPointFlyingAlienState(_bedService, _navMeshAgent));

        _stateMachine.AddState<WanderFlyingAlienState>(
            new WanderFlyingAlienState(_navMeshAgent));

        _stateMachine.AddState<AttackFlyingAlienState>(
            new AttackFlyingAlienState(_navMeshAgent , _projectilePrefab, 
                _projectileSpawnPoint, _animator, _fireRate, _projectileSpeed));
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
        if (other.CompareTag("Player") && !_playerInRange)
        {
            _playerInRange = true;
            _playerTransform = other.transform;
            PlayerDetected?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _playerInRange)
        {
            _playerInRange = false;
            _playerTransform = null;
            PlayerLost?.Invoke();
        }
    }

    protected override void Die()
    {
        Instantiate(_brokenPartsPrefab, _projectileSpawnPoint.position, transform.rotation);

        Destroy(gameObject);
    }
}