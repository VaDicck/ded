using UnityEngine.AI;

public class HuntFleeingAlienState : IChickenState
{
    private NavMeshAgent _agent;
    
    public void Enter()
    {
        _agent.isStopped = true;
    }
    
    public void Tick()
    {
    }
    
    public void Exit()
    {
        _agent.isStopped = false;
    }
}