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

    public void StartAttack()
    {
        _canAttack = true;
        _nextFireTime = Time.time;
    }

    public void Idle()
    {
        _canAttack = false;
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
        TryShoot();
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

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            targetRotation, 
            Time.deltaTime * _rotationSpeed);
    }

    private void TryShoot()
    {
        if (Time.time < _nextFireTime) return;

        Shoot();
        _nextFireTime = Time.time + _fireRate;
    }

    private void Shoot()
    {
        if (_currentTarget == null) return;

        Vector3 spawnPos = _projectileSpawnPoint.position;
        Vector3 direction = (_currentTarget.transform.position - spawnPos).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);

        GameObject projectileGO = Instantiate(_projectilePrefab, spawnPos, rotation);

        projectileGO.GetComponent<Projectile>().Launch(direction, _projectileSpeed, 60);
    }
}