using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class MutatedChicken : ChickenBase
{
    [Header("Night attack")]
    [SerializeField] private float _attackDamage = 20f;
    [SerializeField] private float _attackInterval = 1.2f;
    [SerializeField] private float _attackRange = 2.5f;

    private AlienService _alienService;

    private bool _isNight = false;
    private float _nextHuntCheckTime;

    private const float _huntCheckInterval = 1.5f;

    [Inject]
    private void Init(AlienService alienService)
    {
        _alienService = alienService;
    }

    protected override void OnDayStart()
    {
        _isNight = false;

        if (_stateMachine.CurrentState is HuntFleeingAlienState)
        {
            _stateMachine.SetState<WanderChickenState>();
        }
        else
        {
            _stateMachine.SetState<IdleChickenState>();
        }
    }

    protected override void OnDayEnd()
    {
        _isNight = true;

        _stateMachine.SetState<HuntFleeingAlienState>();
    }

    protected override void AddStates()
    {
        _stateMachine = new StateMachine<IChickenState>(new IdleChickenState());
        _stateMachine.AddState(new WanderChickenState(transform, _navMeshAgent, _wanderRadius));
        _stateMachine.AddState(new PeckChickenState(_navMeshAgent));
        _stateMachine.AddState(new HuntFleeingAlienState(
            _navMeshAgent,
            transform,
            _alienService,
            _attackDamage,
            _attackInterval,
            _attackRange
        ));
    }

    protected override void Subscribe()
    {
        _stateMachine.GetState<IdleChickenState>().Performed += () =>
            _stateMachine.SetState<WanderChickenState>();

        _stateMachine.GetState<PeckChickenState>().Performed += () =>
            _stateMachine.SetState<WanderChickenState>();

        _stateMachine.GetState<HuntFleeingAlienState>().Performed += () =>
            _stateMachine.SetState<WanderChickenState>();
    }

    protected override void Update()
    {
        base.Update();

        if (_isNight && Time.time >= _nextHuntCheckTime)
        {
            if (HasAnyFleeingAlienInGarden() && _stateMachine.CurrentState is not HuntFleeingAlienState)
            {
                _stateMachine.SetState<HuntFleeingAlienState>();
            }
            else if (!HasAnyFleeingAlienInGarden() && _stateMachine.CurrentState is HuntFleeingAlienState)
            {
                _stateMachine.SetState<WanderChickenState>();
            }

            _nextHuntCheckTime = Time.time + _huntCheckInterval;
        }
    }

    private bool HasAnyFleeingAlienInGarden()
    {
        if (_alienService.AliensEmpty) return false;

        foreach (var alien in _alienService.Aliens)
        {
            if (alien.Type == AlienType.Fleeing && IsInGarden(alien.transform.position))
                return true;
        }
        return false;
    }

    private bool IsInGarden(Vector3 position)
    {
        int gardenIndex = NavMesh.GetAreaFromName("Garden");
        int gardenMask = gardenIndex >= 0 ? (1 << gardenIndex) : NavMesh.AllAreas;
        return NavMesh.SamplePosition(position, out _, 1f, gardenMask);
    }
}