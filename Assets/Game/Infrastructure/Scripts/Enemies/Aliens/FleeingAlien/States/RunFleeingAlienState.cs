using System;
using UnityEngine;
using UnityEngine.AI;

public class RunFleeingAlienState : IFleeingAlienState
{ 
    public event Action Performed;
    
    private Transform _destination;
    private Transform _transform;
    private float _speed;
    private float _stopDistance = 3f;
    private NavMeshAgent _navMeshAgent; 

    public RunFleeingAlienState(NavMeshAgent navMeshAgent, Transform transform, float speed = 8f)
    {
        _navMeshAgent = navMeshAgent;
        _transform = transform;
        _speed = speed;
    }
    
    public void SetDestination(Transform destination)
    {
        _destination  = destination;
    }

    public void Enter()
    {
        if (_destination == null || _navMeshAgent == null)
            return;
        
        _navMeshAgent.SetDestination(_destination.position);
        _navMeshAgent.isStopped = false; 
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