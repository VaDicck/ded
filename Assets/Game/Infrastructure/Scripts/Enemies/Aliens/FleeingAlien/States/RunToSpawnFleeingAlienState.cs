using System;
using UnityEngine;
using UnityEngine.AI;

public class RunToSpawnFleeingAlienState : IFleeingAlienState
{
    public event Action Performed;

    private float _stopDistance = 0f;
    private NavMeshAgent _navMeshAgent;

    private Transform _spawnPoint;

    public RunToSpawnFleeingAlienState(NavMeshAgent navMeshAgent, Transform spawnPoint)
    {
        _navMeshAgent = navMeshAgent;
        _spawnPoint = spawnPoint;
    }

    public void Enter()
    {
        _navMeshAgent.updateRotation = true;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_spawnPoint.position);
    }

    public void Tick()
    {
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _stopDistance)
        {
            Performed?.Invoke();
        }
    }

    public void Exit()
    {
        _navMeshAgent.isStopped = true;
    }
}
