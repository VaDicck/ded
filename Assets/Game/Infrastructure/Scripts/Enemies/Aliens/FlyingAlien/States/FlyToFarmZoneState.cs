using System;
using UnityEngine;
using UnityEngine.AI;

public class FlyToFarmZoneState : IFlyingAlienState
{
    public event Action Performed;
    
    private NavMeshAgent _navMeshAgent;
    private int _farmZoneAreaIndex;
    private float _stopDistance = 1.5f;
    private Transform _destination;
    
    private float _targetHeight = 5f;
    private float _currentHeight = 0f;
    private float _heightSmoothTime = 0.5f;
    private float _heightVelocity;
    
    private float _sinAmplitude = 0.4f;   
    private float _sinFrequency = 3f; 
    private float _sinTime = 0f;         
    private bool _isCruising = false;     
    
    public FlyToFarmZoneState(NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
        
        _farmZoneAreaIndex = NavMesh.GetAreaFromName("FarmZone");
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
        
        if (IsOnFarmZone())
        {
            Performed?.Invoke();
        }
        
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _stopDistance)
        {
            Performed?.Invoke();
        }
    }
    
    public void Exit()
    {
        _navMeshAgent.isStopped = true;
    }
    
    private bool IsOnFarmZone()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(_navMeshAgent.transform.position, out hit, 10f, NavMesh.AllAreas))
        {
            int areaIndex = GetAreaIndexFromMask(hit.mask);
            return areaIndex == _farmZoneAreaIndex;
        }
        return false;
    }
    
    private int GetAreaIndexFromMask(int mask)
    {
        for (int i = 0; i < 32; i++)
        {
            if ((mask & (1 << i)) != 0)
                return i;
        }
        return -1;
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