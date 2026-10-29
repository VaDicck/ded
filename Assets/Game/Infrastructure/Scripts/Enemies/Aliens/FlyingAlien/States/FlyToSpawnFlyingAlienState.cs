using System;
using UnityEngine;
using UnityEngine.AI;

public class FlyToSpawnFlyingAlienState : IFlyingAlienState
{
    public event Action Performed;

    private readonly NavMeshAgent _navMeshAgent;
    private readonly Transform _spawnPoint;
    private readonly float _stopDistance = 1f;
    
    public FlyToSpawnFlyingAlienState(Transform spawnPoint, NavMeshAgent navMeshAgent)
    {
        _spawnPoint = spawnPoint;
        _navMeshAgent = navMeshAgent;
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
}
