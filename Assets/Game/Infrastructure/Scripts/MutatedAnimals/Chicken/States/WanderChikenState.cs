using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WanderChickenState : IChickenState
{
    private readonly Transform _transformChicken;
    private readonly NavMeshAgent _agent;
    private readonly float _wanderRadius;
    private readonly float _minWanderTime = 2f;
    private readonly float _maxWanderTime = 6f;

    private float _nextWanderTime;

    public WanderChickenState(Transform chicken, NavMeshAgent agent, float wanderRadius = 15f)
    {
        _transformChicken = chicken;
        _agent = agent;
        _wanderRadius = wanderRadius;
    }

    public void Enter()
    {
        _agent.isStopped = false;
        SetRandomDestination();
    }

    public void Tick()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            //TODO idle state
            if (Time.time >= _nextWanderTime)
            {
                SetRandomDestination();
            }
        }
    }

    //TODO проходит насквозь дырки
    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _wanderRadius;
        randomDirection += _transformChicken.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _wanderRadius * 1.5f, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }

        _nextWanderTime = Time.time + Random.Range(_minWanderTime, _maxWanderTime);
    }
}