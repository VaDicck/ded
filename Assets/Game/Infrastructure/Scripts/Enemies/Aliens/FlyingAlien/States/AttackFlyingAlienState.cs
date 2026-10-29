using System;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

public class AttackFlyingAlienState : IFlyingAlienState
{
    public event Action Performed;

    private readonly NavMeshAgent _navMeshAgent;
    private readonly GameObject _projectilePrefab;
    private readonly Transform _projectileSpawnPoint;
    private readonly float _fireRate;
    private readonly float _projectileSpeed;
    
    private readonly float _shotDelay = 0.79f;
    private float _shotScheduledTime = -1f;

    private Transform _player;
    private float _nextFireTime;
    
    private Animator _animator;

    public AttackFlyingAlienState(NavMeshAgent navMeshAgent, GameObject projectilePrefab, 
                                  Transform projectileSpawnPoint, Animator animator,
                                  float fireRate, float projectileSpeed)
    {
        _animator = animator;
        _navMeshAgent = navMeshAgent;
        _projectilePrefab = projectilePrefab;
        _projectileSpawnPoint = projectileSpawnPoint;
        _fireRate = fireRate;
        _projectileSpeed = projectileSpeed;
    }

    public void SetPlayer(Transform player)
    {
        _player = player;
    }

    public void Enter()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.updateRotation = false;
    }

    public void Tick()
    {
        if (_player == null) return;

        Vector3 direction = (_player.position - _navMeshAgent.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _navMeshAgent.transform.rotation = Quaternion.Slerp(
                _navMeshAgent.transform.rotation, targetRotation, Time.deltaTime * 8f);
        }

        if (Time.time >= _nextFireTime && _shotScheduledTime < 0)
        {
            _animator.SetTrigger("Attack");        
            _shotScheduledTime = Time.time + _shotDelay;   
            _nextFireTime = Time.time + _fireRate;        
        }

        if (_shotScheduledTime > 0 && Time.time >= _shotScheduledTime)
        {
            Shoot();
            _shotScheduledTime = -1f;         
        }
    }

    private void Shoot()
    {
        Vector3 spawnPos = _projectileSpawnPoint.position;
        Vector3 dir = (_player.position - spawnPos).normalized;
        Quaternion rot = Quaternion.LookRotation(dir) * Quaternion.Euler(0, -90, 0);

        GameObject projectileGO = Object.Instantiate(_projectilePrefab, spawnPos, rot);

        if (projectileGO.TryGetComponent<Projectile>(out Projectile projectile))
        {
            projectile.Launch(dir, _projectileSpeed, 0);
        }
    }
}