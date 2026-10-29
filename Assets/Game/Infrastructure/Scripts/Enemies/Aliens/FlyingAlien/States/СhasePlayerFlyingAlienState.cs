using System;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerFlyingAlienState : IFlyingAlienState
{
    public event Action Performed;

    private readonly NavMeshAgent _navMeshAgent;
    private readonly float _minShootDistance;  
    
    private Transform _player;

    public ChasePlayerFlyingAlienState(NavMeshAgent navMeshAgent, float minShootDistance)
    {
        _navMeshAgent = navMeshAgent;
        _minShootDistance = minShootDistance;
    }

    public void SetPlayer(Transform player)
    {
        _player = player;
    }

    public void Enter()
    {
        _navMeshAgent.updateRotation = true;
        _navMeshAgent.isStopped = false;

        _navMeshAgent.SetDestination(_player.position);
    }

    public void Tick()
    {
        _navMeshAgent.SetDestination(_player.position);

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _minShootDistance)
        {
            Performed?.Invoke();
        }
    }

    public void Exit()
    {
        _navMeshAgent.isStopped = true;
    }
}