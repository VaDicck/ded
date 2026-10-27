using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class FlyToPointFlyingAlienState : IFlyingAlienState
{
    public event Action Performed;

    private readonly BedService _bedService;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly float _stopDistance = 1.5f;
    

    public FlyToPointFlyingAlienState(BedService bedService, NavMeshAgent navMeshAgent)
    {
        _bedService = bedService;
        _navMeshAgent = navMeshAgent;
    }

    public void Enter()
    {
        _navMeshAgent.updateRotation = true;
        _navMeshAgent.isStopped = false;

        if (TrySelectRandomBed(out Bed targetBed))
        {
            _navMeshAgent.SetDestination(targetBed.transform.position);
        }
        else
        {
            Performed?.Invoke();
        }
    }

    public void Tick()
    {
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _stopDistance)
        {
            Performed?.Invoke();
        }
    }

    private bool TrySelectRandomBed(out Bed targetBed)
    {
        targetBed = null;

        List<Bed> beds = _bedService.GetBedsByState<PlantedBedState>();

        if (beds.Count > 0)
        {
            targetBed = beds[UnityEngine.Random.Range(0, beds.Count)];
            return true;
        }

        return false;
    }
}