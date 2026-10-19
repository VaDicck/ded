using System;
using R3;
using UnityEngine.AI;

public class RunToBedFleeingAlienState : IFleeingAlienState
{ 
    public event Action<Bed> Performed;
    public event Action RunSpawn;
    
    private Bed _bed;
    private float _stopDistance = 3f;
    private NavMeshAgent _navMeshAgent; 
    
    private BedService _bedService;

    public RunToBedFleeingAlienState(NavMeshAgent navMeshAgent, BedService bedService)
    {
        _navMeshAgent = navMeshAgent;
        _bedService = bedService;

        _bedService.CanAttackBeds.Subscribe(RunToSpawn);
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
        //TODO надо бежать к другой грядке
        if (!_navMeshAgent.pathPending
            && _navMeshAgent.remainingDistance <= _stopDistance
            && _bed.CurrentState is PlantedBedState)
            Performed?.Invoke(_bed);
    }

    public void Exit()
    {
        _navMeshAgent.isStopped = true;
    }

    private void RunToSpawn(bool CanAttack)
    {
        if(!CanAttack)
            RunSpawn?.Invoke();
    }
}