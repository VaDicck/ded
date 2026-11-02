using UnityEditor.Animations;
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
    [SerializeField] protected float _peckDuration = 4f;
    
    [SerializeField] protected Animator _animator;
    
    [SerializeField] private Transform _spawnPoint;
    
    [Header("Audio")]
    [SerializeField] protected AudioSource _audio;
    [SerializeField] protected AudioClip _idleSound;

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
        _audio.clip = _idleSound;
        _audio.Play();
        
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
        if (Time.time >= _nextPeckCheckTime)
        {
            if (Random.value < _peckProbability)
            {
                OnPeck();
                
                _animator.SetTrigger("Peck");
                _stateMachine.SetState<PeckChickenState>();
            }
            _nextPeckCheckTime = Time.time + _peckCheckInterval;
        }
    }

    protected virtual void OnPeck() { }
    
    protected void Walk()
    {
        _audio.clip = _idleSound;
        _audio.Play();
        
        _animator.SetTrigger("Walk");
        _stateMachine.SetState<WanderChickenState>();
    }

    protected abstract void AddStates();

    protected abstract void Subscribe();

    private void OnDestroy()
    { 
        _dayCycleService.DayStarted -= OnDayStart;
        _dayCycleService.DayEnded -= OnDayEnd;
    }
}