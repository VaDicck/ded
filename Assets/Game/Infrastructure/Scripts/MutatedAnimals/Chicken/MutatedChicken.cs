using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class MutatedChicken : ChickenBase
{
    public event Action AlienDetected;
    public event Action AlienLost;

    public List<AlienBase> Aliens => _aliens;

    [Header("Night attack")]
    [SerializeField] private float _attackDamage = 20f;
    [SerializeField] private float _attackInterval = 1.2f;
    [SerializeField] private float _attackRange = 2.5f;

    private float _nextHuntCheckTime;

    private const float _huntCheckInterval = 1.5f;

    private List<AlienBase> _aliens = new List<AlienBase>();

    private int _alienLayer;

    public class Factory : PlaceholderFactory<MutatedChicken> { }

    private void Awake()
    {
        _alienLayer = LayerMask.NameToLayer("Enemies");
    }

    protected override void OnDayStart()
    {
        UnsubscribeOnNight();
        SubscribeOnDay();

        _stateMachine.SetState<IdleChickenState>();
    }

    protected override void OnDayEnd()
    {
        UnsubscribeOnDay();
        SubscribeOnNight();

        _stateMachine.SetState<HuntFleeingAlienState>();
    }

    protected override void AddStates()
    {
        _stateMachine = new StateMachine<IChickenState>(new IdleChickenState());
        _stateMachine.AddState(new WanderChickenState(transform, _navMeshAgent, _wanderRadius));
        _stateMachine.AddState(new PeckChickenState(_navMeshAgent));
        
        _stateMachine.AddState(new HuntFleeingAlienState());
        _stateMachine.AddState(new ChaseFleeingAlienState(
    this,
    _navMeshAgent,
    _attackRange,
    _attackDamage,
    _attackInterval, _animator));
    }

    private void SubscribeOnDay()
    {
        _stateMachine.GetState<IdleChickenState>().Performed += Walk;

        _stateMachine.GetState<PeckChickenState>().Performed += Walk;
    }

    private void UnsubscribeOnDay()
    {
        _stateMachine.GetState<IdleChickenState>().Performed -= Walk;

        _stateMachine.GetState<PeckChickenState>().Performed -= Walk;
    }

    private void SubscribeOnNight()
    {
        _stateMachine.GetState<IdleChickenState>().Performed += Walk;

        _stateMachine.GetState<PeckChickenState>().Performed += Walk;
    }

    private void UnsubscribeOnNight()
    {
        _stateMachine.GetState<IdleChickenState>().Performed -= Walk;

        _stateMachine.GetState<PeckChickenState>().Performed -= Walk;
    }

    protected override void Subscribe()
    {
        SubscribeOnNight();
    }

    protected void Update()
    {
        _stateMachine.Tick();

        if (!_dayCycleService.IsDay)
        {
            return;
        }
        
        PeckCheck();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _alienLayer)
        {
            if (!other.gameObject.TryGetComponent<AlienBase>(out var alien))
                return;

            _stateMachine.SetState<ChaseFleeingAlienState>();
            _aliens.Add(alien);
            this.LogError($"Detected Alien {_aliens.Count}");
            alien.Death += RemoveAlien;
            AlienDetected?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _alienLayer)
        {
            if (!other.gameObject.TryGetComponent<AlienBase>(out var alien))
                return;

            RemoveAlien(alien);
        }
    }

    private void RemoveAlien(AlienBase alienBase)
    {
        this.LogError($"RemoveAlien, count {_aliens.Count}");
        _aliens.Remove(alienBase);
        AlienLost?.Invoke();

        if (_aliens.Count != 0) return;

        Walk();
    }

    private bool IsInGarden(Vector3 position)
    {
        int gardenIndex = NavMesh.GetAreaFromName("Garden");
        int gardenMask = gardenIndex >= 0 ? (1 << gardenIndex) : NavMesh.AllAreas;
        return NavMesh.SamplePosition(position, out _, 1f, gardenMask);
    }
}