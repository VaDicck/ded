using System;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

public class AttackFlyingAlienState : IFlyingAlienState
{
    public event Action Performed;
    public event Action PlayerTooFar;

    private readonly NavMeshAgent _navMeshAgent;
    private readonly GameObject _projectilePrefab;
    private readonly Transform _projectileSpawnPoint;
    private readonly float _fireRate;
    private readonly float _projectileSpeed;
    private readonly float _minShootDistance;
    
    private readonly float _shotDelay = 0.79f;
    private float _shotScheduledTime = -1f;

    private int _countShot;

    private Transform _player;
    private float _nextFireTime;
    
    private Animator _animator;
    
    private AudioSource _audioSource;
    private AudioClip _attack1Sound;
    private AudioClip _attack2Sound;
    private AudioClip _attack3Sound;

    public AttackFlyingAlienState(NavMeshAgent navMeshAgent, GameObject projectilePrefab, 
                                  Transform projectileSpawnPoint, Animator animator,
                                  float fireRate, float projectileSpeed, float minShootDistance,
                                  AudioSource audioSource,
                                  AudioClip attack1Sound, AudioClip attack2Sound, AudioClip attack3Sound)
    {
        _animator = animator;
        _navMeshAgent = navMeshAgent;
        _projectilePrefab = projectilePrefab;
        _projectileSpawnPoint = projectileSpawnPoint;
        _fireRate = fireRate;
        _projectileSpeed = projectileSpeed;
        _minShootDistance = minShootDistance;
        _audioSource = audioSource;
        _attack1Sound = attack1Sound;
        _attack2Sound = attack2Sound;
        _attack3Sound = attack3Sound;
    }

    public void SetPlayer(Transform player)
    {
        _player = player;
    }

    public void Enter()
    {
        _countShot = 0;
        _navMeshAgent.isStopped = true;
        _navMeshAgent.updateRotation = false;
    }

    public void Tick()
    {
        float distanceToPlayer = Vector3.Distance(_navMeshAgent.transform.position, _player.position);
        
        Vector3 direction = (_player.position - _navMeshAgent.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _navMeshAgent.transform.rotation = Quaternion.Slerp(
                _navMeshAgent.transform.rotation, targetRotation, Time.deltaTime * 8f);
        }

        if (_countShot > 1 && distanceToPlayer > _minShootDistance)
        {
            PlayerTooFar?.Invoke();
            return;
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
        _countShot++;
        Vector3 spawnPos = _projectileSpawnPoint.position;
        Vector3 dir = (_player.position - spawnPos).normalized;
        Quaternion rot = Quaternion.LookRotation(dir) * Quaternion.Euler(0, -90, 0);

        PlayAttackAudio();
        
        GameObject projectileGO = Object.Instantiate(_projectilePrefab, spawnPos, rot);

        if (projectileGO.TryGetComponent<Projectile>(out Projectile projectile))
        {
            projectile.Launch(dir, _projectileSpeed, 0);
        }
    }

    private void PlayAttackAudio()
    {
        int currentAttackIndex = _countShot % 3;

        switch (currentAttackIndex)
        {
            case 0:
                _audioSource.PlayOneShot(_attack1Sound);
                break;
            case 1:
                _audioSource.PlayOneShot(_attack2Sound);
                break;
            case 2:
                _audioSource.PlayOneShot(_attack3Sound);
                break;
        }
    }
}