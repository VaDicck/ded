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
        _stateMachine.GetState<FlyToFlyingAlienState>().Performed
            += () => FlyTo(_bed);
        _stateMachine.GetState<FlyToFlyingAlienState>().Performed
            += _stateMachine.SetState<CirclingOverFlyingAlienState>;
        _stateMachine.GetState<CirclingOverFlyingAlienState>().Performed
            += _stateMachine.SetState<AttackFlyingAlienState>;
        _stateMachine.GetState<AttackFlyingAlienState>().Performed
            += _stateMachine.SetState<CirclingOverFlyingAlienState>;
    }

    protected override void AddStates()
    {
        _stateMachine = new StateMachine<IFlyingAlienState>(new FlyToFlyingAlienState(_navMeshAgent));
        _stateMachine.AddState<CirclingOverFlyingAlienState>(new CirclingOverFlyingAlienState());
        _stateMachine.AddState<AttackFlyingAlienState>(new AttackFlyingAlienState(_navMeshAgent, _playerTransform, 15f));
    }

    private void FlyTo(Transform target)
    {
        _stateMachine.SetState<FlyToFlyingAlienState>();
                
        _stateMachine.GetState<FlyToFlyingAlienState>().SetDestination(target);
    }
}