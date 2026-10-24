using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Chicken : MonoBehaviour
{
    [Header("NavMesh Settings")]
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private float _wanderRadius = 15f;
    [SerializeField] private float _peckProbability = 0.25f;
    [SerializeField] private float _peckCheckInterval = 4f;
    
    [SerializeField] private Transform _spawnPoint;

    private StateMachine<IChickenState> _stateMachine;

    private DayCycleService _dayCycleService;

    private float _nextPeckCheckTime;

    [Inject]
    private void Init(DayCycleService dayCycleService)
    {
        _dayCycleService = dayCycleService;
    }

    private void Start()
    {
        SetSpawnPoint(_spawnPoint);
        
        AddStates();
        Subscribe();

        _nextPeckCheckTime = Time.time + _peckCheckInterval;
    }

    private void Update()
    {
        _stateMachine.Tick();

        PeckCheck();
    }

    private void PeckCheck()
    {
        if (Time.time >= _nextPeckCheckTime && _stateMachine.CurrentState is WanderChickenState)
        {
            if (Random.value < _peckProbability)
            {
                _stateMachine.SetState<PeckChickenState>();
            }
            _nextPeckCheckTime = Time.time + _peckCheckInterval;
        }
    }
    
    private void SetSpawnPoint(Transform spawnPoint)
    {
        _navMeshAgent.Warp(spawnPoint.position);
        _spawnPoint = spawnPoint;
    }

    private void AddStates()
    {
        _stateMachine = new StateMachine<IChickenState>(new IdleChickenState());
        _stateMachine.AddState(new WanderChickenState(transform, _navMeshAgent, _wanderRadius));
        _stateMachine.AddState(new PeckChickenState(_navMeshAgent));
    }

    private void Subscribe()
    {
        _stateMachine.GetState<IdleChickenState>().Performed += () =>
        {
            _stateMachine.SetState<WanderChickenState>();
        };
        
        _stateMachine.GetState<PeckChickenState>().Performed += () =>
        {
            _stateMachine.SetState<WanderChickenState>();
        };
    }
}