using System;
using UnityEngine;
using UnityEngine.AI;

public class PeckChickenState : IChickenState
{
    public event Action Performed;

    private readonly NavMeshAgent _agent;
    private readonly float _peckDuration = 4f;
    private float _peckEndTime;

    public PeckChickenState(NavMeshAgent agent)
    {
        _agent = agent;
    }

    public void Enter()
    {
        _agent.isStopped = true;
        _peckEndTime = Time.time + _peckDuration;

        this.Log("КЛЮЕТ");
    }

    public void Exit()
    {
        _agent.isStopped = false;
    }

    public void Tick()
    {
        if (Time.time >= _peckEndTime)
        {
            Performed?.Invoke();
        }
    }
}