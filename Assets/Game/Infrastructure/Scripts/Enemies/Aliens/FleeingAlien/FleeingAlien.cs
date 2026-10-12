using UnityEngine;
using Zenject;

public class FleeingAlien : Alien<IFleeingAlienState>
{
    public class Factory : PlaceholderFactory<FleeingAlien>
    {
        
    }

    protected override void AddStates()
    {
        _stateMachine = new StateMachine<IFleeingAlienState>(new IdleFleeingAlienState());
        _stateMachine.AddState<RunFleeingAlienState>(new RunFleeingAlienState(_navMeshAgent));
        _stateMachine.AddState<DiggingBedFleeingAlienState>(new DiggingBedFleeingAlienState());
    }
    
    protected override void Subscribe()
    {
        _stateMachine.GetState<IdleFleeingAlienState>().Performed 
            += () => RunTo(_bed);
        _stateMachine.GetState<RunFleeingAlienState>().Performed 
            +=  _stateMachine.SetState<DiggingBedFleeingAlienState>;
        _stateMachine.GetState<DiggingBedFleeingAlienState>().Performed 
            += () => RunTo(_spawnPoint);
        _stateMachine.GetState<RunFleeingAlienState>().Performed 
            +=  Lose;
    }

    private void RunTo(Transform target)
    {
        _stateMachine.GetState<RunFleeingAlienState>().SetDestination(target);
        
        _stateMachine.SetState<RunFleeingAlienState>();
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    private void Lose()
    {
        //Destroy(gameObject);
    }
}