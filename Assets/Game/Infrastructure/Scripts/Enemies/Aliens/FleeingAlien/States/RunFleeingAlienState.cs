using System;
using UnityEngine;

public class RunFleeingAlienState : IFleeingAlienState
{ 
    public event Action Performed;
    
    private Transform _destination;
    private Transform _transform;
    private float _speed;
    private float _stopDistance = 3f;

    public RunFleeingAlienState(Transform transform, float speed = 8f)
    {
        _transform = transform;
        _speed = speed;
    }
    
    public void SetDestination(Transform destination)
    {
        _destination  = destination;
    }

    public void Tick()
    {
        if (_destination == null || _transform == null)
            throw new ArgumentException("No destination or transform provided");
        
        Vector3 direction = (_destination.position - _transform.position).normalized;
        _transform.position += direction * _speed * Time.deltaTime;
        
        float distance = Vector3.Distance(_transform.position, _destination.position);
        
        if (distance <= _stopDistance)
        {
            Performed?.Invoke(); 
        }
    }
}