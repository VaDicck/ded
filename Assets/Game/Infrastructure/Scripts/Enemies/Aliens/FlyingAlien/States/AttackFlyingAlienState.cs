using System;
using UnityEngine;
using UnityEngine.AI;

public class AttackFlyingAlienState : IFlyingAlienState
{
    public event Action Performed;

    private NavMeshAgent _navMeshAgent;
    private Transform _playerTransform;
    private float _attackRange;

    public AttackFlyingAlienState(NavMeshAgent navMeshAgent, Transform playerTransform, float attackRange)
    {
        _navMeshAgent = navMeshAgent;
        _playerTransform = playerTransform;
        _attackRange = attackRange;
    }

    public void Enter()
    {
        _navMeshAgent.isStopped = true;
        Attack();
    }

    public void Exit()
    {
        _navMeshAgent.isStopped = false;
    }

    private void Attack()
    {
        if (IsPlayerInRange())
        {
            Performed?.Invoke();
        }
    }

    private bool IsPlayerInRange()
    {
        if (_playerTransform == null) return false;

        float distanceToPlayer = Vector3.Distance(_navMeshAgent.transform.position, _playerTransform.position);
        
        return distanceToPlayer <= _attackRange;
    }

}