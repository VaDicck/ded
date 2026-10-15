using System;
using UnityEngine.AI;

public class RunToBedFleeingAlienState : IFleeingAlienState
{ 
    public event Action<Bed> Performed;
    
    private Bed _bed;
    private float _stopDistance = 3f;
    private NavMeshAgent _navMeshAgent; 

    public RunToBedFleeingAlienState(NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
    }
    
    public void SetBed(Bed bed)
    {
        _bed  = bed;
    }

    public void Enter()
    {
        _navMeshAgent.isStopped = false; 
        _navMeshAgent.SetDestination(_bed.transform.position);
    }

    public void Tick()
    {
        //TODO canAttack, maybe. This class need bedService?
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _stopDistance
                                       && _bed.CurrentState is PlantedBedState)
            Performed?.Invoke(_bed);
    }

    public void Exit()
    {
        _navMeshAgent.isStopped = true;
    }
}