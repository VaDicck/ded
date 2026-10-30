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
    [SerializeField] private float _nightSpeed = 5f;

    private List<AlienBase> _aliens = new List<AlienBase>();

    private int _alienLayer;
    private float _daySpeed;

    public class Factory : PlaceholderFactory<MutatedChicken> { }

    private void Awake()
    {
        _alienLayer = LayerMask.NameToLayer("Enemies");
        _daySpeed = _navMeshAgent.speed;
    }

    protected override void OnDayStart()
    {
        _stateMachine.SetState<IdleChickenState>();
        _navMeshAgent.speed = _daySpeed;
    }

    protected override void OnDayEnd()
    {
        _stateMachine.SetState<WanderChickenState>(); 
        _navMeshAgent.speed = _nightSpeed;
    }

    protected override void AddStates()
    {
        _stateMachine = new StateMachine<IChickenState>(new IdleChickenState(_animator));
        _stateMachine.AddState(new WanderChickenState(transform, _navMeshAgent, _wanderRadius));
        _stateMachine.AddState(new PeckChickenState(_navMeshAgent, _peckDuration));
        
        _stateMachine.AddState(new ChaseFleeingAlienState(
            this,
            _navMeshAgent,
            _attackRange,
            _attackDamage,
            _attackInterval, _animator));
    }

    protected override void Subscribe()
    {
        _stateMachine.GetState<IdleChickenState>().Performed += Walk;
        _stateMachine.GetState<PeckChickenState>().Performed += Walk;
        _stateMachine.GetState<ChaseFleeingAlienState>().Performed += Walk;
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

    private void OnAttack(float numberAttack)
    {
        _stateMachine.GetState<ChaseFleeingAlienState>().AttackEnd(numberAttack);
    }

    private void RemoveAlien(AlienBase alienBase)
    {
        _aliens.Remove(alienBase);
        AlienLost?.Invoke();

        if (_aliens.Count != 0) return;

        Walk();
    }
}