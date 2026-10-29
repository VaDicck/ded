using System;
using R3;
using UnityEngine;
using UnityEngine.AI;

public class RunToBedFleeingAlienState : IFleeingAlienState
{ 
    public event Action<Bed> Performed;
    public event Action RunSpawn;
    
    private Bed _bed;
    private readonly float _stopDistance = 0.5f;
    private NavMeshAgent _navMeshAgent; 
    
    private BedService _bedService;
    
    public RunToBedFleeingAlienState(NavMeshAgent navMeshAgent, BedService bedService)
    {
        _navMeshAgent = navMeshAgent;
        _bedService = bedService;
    }
    
    public void SetBed(Bed bed)
    {
        _bed  = bed;
    }

    public void Enter()
    {
        _navMeshAgent.updateRotation = true;
        _navMeshAgent.isStopped = false; 
        
        Vector3 approachPoint = GetRandomApproachPoint();
        _navMeshAgent.SetDestination(approachPoint);
    }

    public void Tick()
    {
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
    
    private Vector3 GetRandomApproachPoint()
    {
        Vector3 bedPos = _bed.transform.position;

        Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
    
        float offsetRadius = 0.3f; 

        Vector3 offset = new Vector3(randomDir.x, 0, randomDir.y) * offsetRadius;
        Vector3 targetPoint = bedPos + offset;

        if (NavMesh.SamplePosition(targetPoint, out NavMeshHit hit, 3f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return targetPoint;
    }
}