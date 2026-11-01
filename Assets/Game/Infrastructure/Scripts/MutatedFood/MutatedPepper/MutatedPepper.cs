using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MutatedPepper : MutatedFood
{
    public class Factory : PlaceholderFactory<MutatedPepper> { }

    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _fireRate = 2f;        
    [SerializeField] private float _projectileSpeed = 10f; 
    [SerializeField] private float _rotationSpeed = 8f;     
    [SerializeField] private float _damageBullet = 30f;     

    private readonly HashSet<AlienBase> _enemiesInRange = new();
    private AlienBase _currentTarget;
    private float _nextFireTime;

    private int _layerAlien;
    private bool _canAttack;
    private bool _isAttacking;
    private int _currentAttackIndex = -1;

    public void StartAttack()
    {
        _canAttack = true;
        _isAttacking = false;
        _nextFireTime = Time.time;
    }

    public void Idle()
    {
        _animator.SetTrigger("Idle");
        _canAttack = false;
        _isAttacking = false;
        _currentTarget = null;
    }

    private void Awake()
    {
        _layerAlien = LayerMask.NameToLayer("Enemies");
    }

    private void Update()
    {
        if(!_canAttack)
            return;
        
        UpdateTarget();
        if(_currentTarget == null)
            return;
        
        LookAtTarget();
        TryStartAttack();
    }
    
    private void TryStartAttack()
    {
        if (_isAttacking || Time.time < _nextFireTime) 
            return;

        StartNewAttack();
    }
    
    private void StartNewAttack()
    {
        _isAttacking = true;

        _currentAttackIndex = (_currentAttackIndex + 1) % 3;

        _animator.SetTrigger($"Attack{_currentAttackIndex + 1}");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _layerAlien)
            return;

        if (other.TryGetComponent<AlienBase>(out AlienBase alien))
        {
            if (_enemiesInRange.Add(alien)) 
            {
                alien.Death += RemoveAlien;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != _layerAlien)
            return;
        
        if (other.TryGetComponent<AlienBase>(out AlienBase alien))
        {
            RemoveAlien(alien);
        }
    }
    
    private void RemoveAlien(AlienBase alienBase)
    {
        if (_enemiesInRange.Remove(alienBase))
        {
            alienBase.Death -= RemoveAlien;
        }
    }
    
    private void UpdateTarget()
    {
        if (_enemiesInRange.Count == 0)
        {
            _currentTarget = null;
            return;
        }

        float minDistance = float.MaxValue;
        AlienBase closest = null;
        Vector3 myPosition = transform.position;

        foreach (var enemy in _enemiesInRange)
        {
            float distance = Vector3.Distance(myPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        _currentTarget = closest;
    }

    private void LookAtTarget()
    {
        if (_currentTarget == null) return;

        Vector3 direction = _currentTarget.transform.position - transform.position;
        direction.y = 0f;                  
        if (direction.sqrMagnitude < 0.001f) return;

        direction.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(direction) * 
                                    Quaternion.Euler(0f, 180f, 0f);
        
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            targetRotation, 
            Time.deltaTime * _rotationSpeed);
    }

    private void Shot()       
    {
        if (_currentTarget == null) return;
        Vector3 spawnPos = _projectileSpawnPoint.position;
        Vector3 direction = (_currentTarget.transform.position - spawnPos).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject projectileGO = Instantiate(_projectilePrefab, spawnPos, rotation);
        projectileGO.GetComponent<Projectile>().Launch(direction, _projectileSpeed, _damageBullet);
    }

    private void EndAttack()
    {
        _isAttacking = false;
        _nextFireTime = Time.time + _fireRate;
        _animator.SetTrigger("Idle");
    }
}