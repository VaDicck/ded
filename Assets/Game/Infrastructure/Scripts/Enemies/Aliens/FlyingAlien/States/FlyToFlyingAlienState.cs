using System;
using UnityEngine;
using UnityEngine.AI;

public class FlyToFlyingAlienState : IFlyingAlienState
{
    public event Action Performed;
    
    private Transform _destination;
    private float _stopDistance = 1.5f;
    private NavMeshAgent _navMeshAgent; 

    public FlyToFlyingAlienState(NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
    }
    
    public void SetDestination(Transform destination)
    {
        _destination  = destination;
    }

    public void Enter()
    {
        if (_destination == null || _navMeshAgent == null)
            return;
        
        _navMeshAgent.isStopped = false; 
        _navMeshAgent.SetDestination(_destination.position);
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