using System;
using UnityEngine;
using UnityEngine.AI;

public class ChasedFlyingAlienState : IFlyingAlienState
{
    public event Action Performed;
    public event Action Attack;
    
    private NavMeshAgent _navMeshAgent;
    private Transform _transform;
    private Transform _playerTransform;
    
    private float _followDistance = 10f;    
    private float _minDistance = 5f;       
    private float _maxDistance = 15f;        
    
    private float _targetHeight = 3f;
    private float _currentHeight = 0f;
    private float _heightSmoothTime = 0.5f;
    private float _heightVelocity;
    
    private float _sinAmplitude = 0.5f;
    private float _sinFrequency = 2f;
    private float _sinTime = 0f;
    private bool _isCruising = false;
    
    private Vector3 _targetPosition;
    
    private float _speed;  
    private float _rotationSpeed; 
    
    private float _attackCooldown = 1.5f; 
    private float _attackTimer = 0f;
    
    public ChasedFlyingAlienState(NavMeshAgent navMeshAgent, Transform playerTransform)
    {
        _navMeshAgent = navMeshAgent;
        
        _speed = _navMeshAgent.speed;
        _rotationSpeed =  _navMeshAgent.angularSpeed;
        
        _transform = navMeshAgent.transform;
        _playerTransform = playerTransform;
    }
    
    public void Enter()
    {
        _currentHeight = _navMeshAgent.baseOffset;
        _isCruising = false;
        _sinTime = 0;
        
        _navMeshAgent.autoBraking = false;
        _navMeshAgent.isStopped = false;
    }
    
    public void Tick()
    {
        UpdateTargetPosition();
        
        UpdateHeight();
        
        MoveToTarget();
        
        LookAtPlayer();
        
        CheckExitConditions();

        TryAttack();
    }
    
    public void Exit()
    {
        _navMeshAgent.isStopped = true;
    }
    
    private void UpdateTargetPosition()
    {
        Vector3 directionFromPlayer = (_transform.position - _playerTransform.position).normalized;
        float currentDistance = Vector3.Distance(_transform.position, _playerTransform.position);
        
        if (currentDistance > _maxDistance)
        {
            _targetPosition = _playerTransform.position + directionFromPlayer * _followDistance;
        }
        else if (currentDistance < _minDistance)
        {
            _targetPosition = _playerTransform.position - directionFromPlayer * _followDistance;
        }
        else
        {
            Vector3 orbitPosition = GetOrbitPosition();
            _targetPosition = orbitPosition;
        }
        
        _targetPosition += GetRandomOffset();
        
        _targetPosition.y = _transform.position.y;
    }
    
    private Vector3 GetOrbitPosition()
    {
        float angle = Time.time * 0.5f;
        Vector3 orbitOffset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * _followDistance;
        
        return _playerTransform.position + orbitOffset;
    }
    
    private Vector3 GetRandomOffset()
    {
        float offsetX = (Mathf.PerlinNoise(Time.time, 0) - 0.5f) * 2f;
        float offsetZ = (Mathf.PerlinNoise(0, Time.time) - 0.5f) * 2f;
        
        return new Vector3(offsetX, 0, offsetZ);
    }
    
    private void UpdateHeight()
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
    
    private void MoveToTarget()
    {
        _navMeshAgent.SetDestination(_targetPosition);
        
        if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial || 
            _navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Vector3 direction = (_targetPosition - _transform.position).normalized;
            Vector3 newPosition = _transform.position + direction * _speed * Time.deltaTime;
            
            if (!Physics.CheckSphere(newPosition, 0.5f))
            {
                _transform.position = newPosition;
            }
        }
    }
    
    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = (_playerTransform.position - _transform.position).normalized;
        directionToPlayer.y = 0; 
        
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
    }
    
    private void CheckExitConditions()
    {
        float distanceToPlayer = Vector3.Distance(_transform.position, _playerTransform.position);
        if (distanceToPlayer > 30f)
        {
            Performed?.Invoke();
        }
        
        RaycastHit hit;
        Vector3 directionToPlayer = (_playerTransform.position - _transform.position).normalized;
        if (Physics.Raycast(_transform.position, directionToPlayer, out hit, 20f))
        {
            if (hit.transform != _playerTransform)
            {
                Performed?.Invoke();
            }
        }
    }

    private void TryAttack()
    {
        _attackTimer -= Time.deltaTime;
    
        if (_attackTimer <= 0f)
        {
            Performed?.Invoke();
        
            _attackTimer = _attackCooldown;
        }
    }
}