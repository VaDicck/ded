using UnityEngine;
using Zenject;

public class FlyingAlien : Alien<IFlyingAlienState>
{
    private Transform _playerTransform;

    public class Factory : PlaceholderFactory<FlyingAlien>
    {

    }

    public void SetPlayer(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    protected override void Subscribe()
    {
        _stateMachine.GetState<IdleFlyingAlienState>().Performed
            += () =>
            {
                _stateMachine.GetState<FlyToFarmZoneState>().SetDestination(_bed);

                _stateMachine.SetState<FlyToFarmZoneState>();
            };
        _stateMachine.GetState<FlyToFarmZoneState>().Performed
            += _stateMachine.SetState<ChasedFlyingAlienState>;
        
        _stateMachine.GetState<ChasedFlyingAlienState>().Attack
            += _stateMachine.SetState<AttackFlyingAlienState>;

        _stateMachine.GetState<AttackFlyingAlienState>().Performed
            += _stateMachine.SetState<ChasedFlyingAlienState>;
    }

    protected override void AddStates()
    {
        _stateMachine = new StateMachine<IFlyingAlienState>(new IdleFlyingAlienState());
        _stateMachine.AddState<FlyToFarmZoneState>(new FlyToFarmZoneState(_navMeshAgent));
        _stateMachine.AddState<ChasedFlyingAlienState>(new ChasedFlyingAlienState(_navMeshAgent, _playerTransform));
        _stateMachine.AddState<FlyToFlyingAlienState>(new FlyToFlyingAlienState(_navMeshAgent));
        _stateMachine.AddState<CirclingOverFlyingAlienState>(new CirclingOverFlyingAlienState());
        _stateMachine.AddState<AttackFlyingAlienState>(new AttackFlyingAlienState(_navMeshAgent, _playerTransform, 15f));
    }

    private void FlyTo(Transform target)
    {
        _stateMachine.GetState<FlyToFlyingAlienState>().SetDestination(target);
        
        _stateMachine.SetState<FlyToFlyingAlienState>();
    }
}