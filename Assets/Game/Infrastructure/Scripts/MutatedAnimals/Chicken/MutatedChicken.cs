using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MutatedChicken : ChickenBase
{
    public event Action AlienDetected;
    public event Action AlienLost;

    public List<AlienBase> Aliens => _aliens;

    [SerializeField] private float _daySpeed;
    
    [Header("Night attack")]
    [SerializeField] private float _attackFistDamage;
    [SerializeField] private float _attackBeakDamage;
    [SerializeField] private float _attackJumpDamage;
    [SerializeField] private float _attackInterval = 1.2f;
    [SerializeField] private float _attackRange = 2.5f;
    [SerializeField] private float _jumpAoERadius = 3.5f;

    private List<AlienBase> _aliens = new List<AlienBase>();

    private int _alienLayer;
    private float _nightSpeed;

    public class Factory : PlaceholderFactory<MutatedChicken> { }

    private void Awake()
    {
        _alienLayer = LayerMask.NameToLayer("Enemies");
        _nightSpeed = _navMeshAgent.speed;
    }

    protected override void OnDayStart()
    {
        _stateMachine.SetState<IdleChickenState>();
        _navMeshAgent.speed = _daySpeed;
    }

    protected override void OnDayEnd()
    {
        Walk();
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
            _attackInterval, _animator, _attackFistDamage, _attackBeakDamage, _attackJumpDamage, _jumpAoERadius));
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

            if (alien.Type == AlienType.Fleeing)
            {
                _stateMachine.SetState<ChaseFleeingAlienState>();
                _aliens.Add(alien);
                alien.Death += RemoveAlien;
                AlienDetected?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _alienLayer)
        {
            if (!other.gameObject.TryGetComponent<AlienBase>(out var alien))
                return;

            if (alien.Type == AlienType.Fleeing)
            {
                RemoveAlien(alien);
            }
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
    
    private void OnDestroy()
    {
        foreach (var alien in _aliens.ToArray())
        {
            if (alien != null)
            {
                alien.Death -= RemoveAlien;
            }
        }
        _aliens.Clear();
    }
}