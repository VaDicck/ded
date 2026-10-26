using UnityEngine.AI;

public class HuntFleeingAlienState : IChickenState
{
    private NavMeshAgent _agent;
    private AlienService _alienService;
    
    public void Enter()
    {
        _agent.isStopped = true;
    }
    
    public void Tick()
    {
        if(_alienService.AliensEmpty)
            return;
        
        
    }
    
    public void Exit()
    {
        _agent.isStopped = false;
    }
}