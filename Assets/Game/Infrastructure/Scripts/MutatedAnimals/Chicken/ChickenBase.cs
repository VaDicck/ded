using UnityEngine;
using UnityEngine.AI;
using Zenject;

public abstract class ChickenBase : MonoBehaviour
{
    [Header("NavMesh Settings")]
    [SerializeField] protected NavMeshAgent _navMeshAgent;
    [SerializeField] protected float _wanderRadius = 15f;
    [SerializeField] private float _peckProbability = 0.25f;
    [SerializeField] private float _peckCheckInterval = 4f;
    
    [SerializeField] private Transform _spawnPoint;

    protected StateMachine<IChickenState> _stateMachine;

    protected DayCycleService _dayCycleService;

    private float _nextPeckCheckTime;

    [Inject]
    private void Init(DayCycleService dayCycleService)
    {
        _dayCycleService = dayCycleService;
    }

    public void WarpTo(Vector3 position)
    {
        _navMeshAgent.Warp(position);
    }
    
    public void SetSpawnPoint(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
    }

    private void Start()
    {
        _dayCycleService.DayStarted += OnDayStart;
        _dayCycleService.DayEnded += OnDayEnd;

        WarpToSpawnPoint();
        
        AddStates();
        
        Subscribe();

        _nextPeckCheckTime = Time.time + _peckCheckInterval;
    }
    
    protected void WarpToSpawnPoint()
    {
        _navMeshAgent.Warp(_spawnPoint.position);
    }

    protected abstract void OnDayStart();

    protected abstract void OnDayEnd();

    protected void PeckCheck()
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
    
    protected void OnIdlePerformed()
    {
        _stateMachine.SetState<WanderChickenState>();
    }

    protected void OnPeckPerformed()
    {
        _stateMachine.SetState<WanderChickenState>();
    }

    protected abstract void AddStates();

    protected abstract void Subscribe();

    private void OnDestroy()
    { 
        _dayCycleService.DayEnded -= OnDayStart;
        _dayCycleService.DayEnded -= OnDayEnd;
    }
}