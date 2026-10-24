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
    
    private readonly int _gardenMask;

    private float _nextWanderTime;

    public WanderChickenState(Transform chicken, NavMeshAgent agent, float wanderRadius = 15f)
    {
        _transformChicken = chicken;
        _agent = agent;
        _wanderRadius = wanderRadius;
        
        int gardenIndex = NavMesh.GetAreaFromName("Garden");
        _gardenMask = gardenIndex >= 0 ? (1 << gardenIndex) : NavMesh.AllAreas;
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

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _wanderRadius;
        randomDirection += _transformChicken.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _wanderRadius * 1.5f, _gardenMask))
        {
            _agent.SetDestination(hit.position);
        }
        else
        {
            _nextWanderTime = Time.time + 0.3f;
            return;
        }

        _nextWanderTime = Time.time + Random.Range(_minWanderTime, _maxWanderTime);
    }
}