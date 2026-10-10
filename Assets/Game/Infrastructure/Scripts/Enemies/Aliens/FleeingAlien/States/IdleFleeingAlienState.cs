using System;
using UnityEngine;

public class IdleFleeingAlienState : IFleeingAlienState
{
    public event Action Performed;

    private readonly float _idleDuration = 3f;
    private float _idleTimer = 0f;
    
    public void Tick()
    {
        _idleTimer += Time.deltaTime;

        if (_idleTimer >= _idleDuration)
        {
            Performed?.Invoke();
        }
    }
}