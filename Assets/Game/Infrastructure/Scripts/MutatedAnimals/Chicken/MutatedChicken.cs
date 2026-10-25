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

        UnsubscribeOnNight();
        SubscribeOnDay();

        _stateMachine.SetState<IdleChickenState>();
    }

    protected override void OnDayEnd()
    {
        _isNight = true;

        UnsubscribeOnDay();
        SubscribeOnNight();

        _stateMachine.SetState<HuntFleeingAlienState>();
    }

    protected override void AddStates()
    {
        base.AddStates();

        _stateMachine.AddState(new HuntFleeingAlienState());
        _stateMachine.AddState(new ChaseFleeingAlienState());
    }

    private void SubscribeOnDay()
    {
        _stateMachine.GetState<IdleChickenState>().Performed += OnIdlePerformed;

        _stateMachine.GetState<PeckChickenState>().Performed += OnPeckPerformed;
    }

    private void UnsubscribeOnDay()
    {
        _stateMachine.GetState<IdleChickenState>().Performed -= OnIdlePerformed;

        _stateMachine.GetState<PeckChickenState>().Performed -= OnPeckPerformed;
    }

    private void SubscribeOnNight()
    {

    }

    private void UnsubscribeOnNight()
    {

    }

    protected override void Subscribe()
    {
        SubscribeOnNight();
    }

    protected override void Update()
    {
        base.Update();

        if (_isNight)
        {

        }
    }

    private void OnIdlePerformed()
    {
        _stateMachine.SetState<WanderChickenState>();
    }

    private void OnPeckPerformed()
    {
        _stateMachine.SetState<WanderChickenState>();
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