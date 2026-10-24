using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Chicken<TState> : MonoBehaviour where TState : IChickenState
{
    [Header("NavMesh Settings")]
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private float wanderRadius = 15f;
    [SerializeField] private float peckProbability = 0.25f;
    [SerializeField] private float peckCheckInterval = 4f;

    private StateMachine<IChickenState> _stateMachine;

    private DayCycleService _dayCycleService;

    private float _nextPeckCheckTime;

    [Inject]
    private void Init(DayCycleService dayCycleService)
    {
        _dayCycleService = dayCycleService;
    }

    private void Awake()
    {
        _navMeshAgent.speed = 1.8f;
        _navMeshAgent.angularSpeed = 300f;
        _navMeshAgent.acceleration = 8f;
        _navMeshAgent.stoppingDistance = 0.5f;
        _navMeshAgent.radius = 0.3f;
        _navMeshAgent.height = 0.6f;
    }

    private void Start()
    {
        AddStates();
        Subscribe();

        _nextPeckCheckTime = Time.time + peckCheckInterval;
    }

    private void Update()
    {
        _stateMachine.Tick();

        // Периодическая проверка на клевание
        if (Time.time >= _nextPeckCheckTime && _stateMachine.CurrentState is WanderChickenState)
        {
            if (Random.value < peckProbability)
            {
                _stateMachine.SetState<PeckChickenState>();
            }
            _nextPeckCheckTime = Time.time + peckCheckInterval;
        }
    }

    protected void AddStates()
    {
        _stateMachine = new StateMachine<IChickenState>(new WanderChickenState(transform, _navMeshAgent, wanderRadius));
        _stateMachine.AddState(new PeckChickenState(_navMeshAgent));
    }

    protected void Subscribe()
    {
        _stateMachine.GetState<PeckChickenState>().Performed += () =>
        {
            _stateMachine.SetState<WanderChickenState>();
        };
    }
}