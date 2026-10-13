using System;
using UnityEngine;
using UnityEngine.AI;

public class FlyToFlyingAlienState : IFlyingAlienState
{
    public event Action Performed;
    
    private Transform _destination;
    private float _stopDistance = 1.5f;
    private NavMeshAgent _navMeshAgent;
    
    private float _targetHeight = 5f;
    private float _currentHeight = 0f;
    private float _heightSmoothTime = 0.5f;
    private float _heightVelocity;
    
    private float _sinAmplitude = 0.4f;   
    private float _sinFrequency = 3f; 
    private float _sinTime = 0f;         
    private bool _isCruising = false;       
    
    public FlyToFlyingAlienState(NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
    }
    
    public void SetDestination(Transform destination)
    {
        _destination = destination;
        _targetHeight = _navMeshAgent.baseOffset;
    }
    
    public void Enter()
    {
        _currentHeight = _navMeshAgent.baseOffset;
        _isCruising = false;
        _sinTime = 0;
        
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_destination.position);
    }
    
    public void Tick()
    {
        Fly();
        
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _stopDistance)
        {
            Performed?.Invoke();
        }
    }
    
    public void Exit()
    {
        _navMeshAgent.isStopped = true;
    }

    private void Fly()
    {
        if (!_isCruising)
        {
            _currentHeight = Mathf.SmoothDamp(_currentHeight, _targetHeight, ref _heightVelocity, _heightSmoothTime);
            
            if (Mathf.Abs(_currentHeight - _targetHeight) < 0.1f)
            {
                _isCruising = true;
                _currentHeight = _targetHeight;
            }
        }
        else
        {
            _sinTime += Time.deltaTime * _sinFrequency;
            float sinValue = Mathf.Sin(_sinTime) * _sinAmplitude;
            _currentHeight = _targetHeight + sinValue;
        }
        
        _navMeshAgent.baseOffset = _currentHeight;
    }
}