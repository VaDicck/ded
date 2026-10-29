using System;
using UnityEngine;
using UnityEngine.AI;

public class PeckChickenState : IChickenState
{
    public event Action Performed;

    private readonly NavMeshAgent _agent;
    private readonly float _peckDuration;
    private float _peckEndTime;

    public PeckChickenState(NavMeshAgent agent, float peckDuration)
    {
        _agent = agent;
        _peckDuration = peckDuration;
    }

    public void Enter()
    {
        _agent.isStopped = true;
        _peckEndTime = Time.time + _peckDuration;
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