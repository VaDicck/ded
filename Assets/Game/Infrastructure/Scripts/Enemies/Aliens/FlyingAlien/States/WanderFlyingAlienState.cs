using System;
using UnityEngine;
using UnityEngine.AI;

public class WanderFlyingAlienState : IFlyingAlienState
{
    public event Action Performed;

    private readonly NavMeshAgent _navMeshAgent;
    private readonly float _stopDistance = 0.5f;
    private readonly float _wanderRadius = 12f; 
    private readonly int _gardenAreaMask;

    public WanderFlyingAlienState(NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;

        int areaIndex = NavMesh.GetAreaFromName("Garden");
        _gardenAreaMask = areaIndex >= 0 ? (1 << areaIndex) : NavMesh.AllAreas;
    }

    public void Enter()
    {
        _navMeshAgent.updateRotation = true;
        _navMeshAgent.isStopped = false;
        PickNewWanderPoint();
    }

    public void Tick()
    {
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _stopDistance)
        {
            PickNewWanderPoint();
        }
    }

    public void Exit()
    {
        _navMeshAgent.isStopped = true;
    }

    private void PickNewWanderPoint()
    {
        Vector3 randomPos = _navMeshAgent.transform.position + UnityEngine.Random.insideUnitSphere * _wanderRadius;

        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, _wanderRadius, _gardenAreaMask))
        {
            _navMeshAgent.SetDestination(hit.position);
        }
    }
}