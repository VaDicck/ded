using System;
using UnityEngine;
using UnityEngine.AI;
using R3;

public class ChaseFleeingAlienState : IChickenState
{
    public event Action Performed;
    
    private readonly MutatedChicken _chicken;      
    private readonly NavMeshAgent _agent;
    private readonly float _attackRange;
    private readonly float _attackFistDamage;
    private readonly float _attackBeakDamage;
    private readonly float _attackJumpDamage;
    private readonly float _attackInterval;
    private readonly float _jumpAoERadius;

    private float _nextAttackTime;
    private AlienBase _target;
    
    private Animator _animator;

    private bool _isMoving;

    private int _countFistAttack = 0;

    public ChaseFleeingAlienState(MutatedChicken chicken, NavMeshAgent agent,
                                  float attackRange, float attackInterval, Animator animator,
                                  float attackFistDamage, float attackBeakDamage, float attackJumpDamage, float jumpAoERadius)
    {
        _chicken = chicken;
        _agent = agent;
        _attackRange = attackRange;
        _attackFistDamage = attackFistDamage;
        _attackBeakDamage = attackBeakDamage;
        _attackJumpDamage = attackJumpDamage;
        _attackInterval = attackInterval;
        _animator = animator;
        _jumpAoERadius = jumpAoERadius;
    }

    public void Enter()
    {
        _nextAttackTime = Time.time;
        _isMoving = false;
        _countFistAttack = 0;
        ChooseClosestTarget();
    }

    public void Tick()
    {
        if (_chicken.Aliens.Count == 0)
        {
            Performed?.Invoke();             
            return;
        }

        if (_target != null && !_target.IsOnGarden.CurrentValue)
        {
            ChooseClosestTarget();
            if (_target == null)
            {
                Performed?.Invoke();   
                return;
            }
        }
        
        if (!_chicken.Aliens.Contains(_target))
        {
            ChooseClosestTarget();
            if (_target == null)
            {
                Performed?.Invoke();   
                return;
            }
        }

        float distance = Vector3.Distance(_chicken.transform.position, _target.transform.position);
        bool shouldMove = distance > _attackRange;

        if (shouldMove != _isMoving)
        {
            _isMoving = shouldMove;

            if (_isMoving)
            {
                _animator.SetTrigger("Walk");
                _agent.isStopped = false;
            }
            else
            {
                _agent.isStopped = true;
                _agent.ResetPath();     
            }
        }

        if (_isMoving)
        {
            _agent.SetDestination(_target.transform.position);
        }
        else 
        {
            RotateTowardsTarget();

            if (Time.time >= _nextAttackTime)
            {
                Attack();
                _nextAttackTime = Time.time + _attackInterval;
            }
        }
    }
    
    private void RotateTowardsTarget()
    {
        if (_target == null) return;

        Vector3 direction = (_target.transform.position - _chicken.transform.position).normalized;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            _chicken.transform.rotation = Quaternion.Slerp(_chicken.transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    public void Exit()
    {
        _agent.ResetPath();   
        _target = null;
        _isMoving = false;
    }

    private void ChooseClosestTarget()
    {
        _target = null;
        float minDistance = float.MaxValue;

        foreach (var alien in _chicken.Aliens)
        {
            if (alien != null)
            {
                float dist = Vector3.Distance(_chicken.transform.position, alien.transform.position);
                if (dist < minDistance && alien.IsOnGarden.CurrentValue)
                {
                    minDistance = dist;
                    _target = alien;
                }
            }
        }
    }

    private void Attack()
    {
        if (_target == null) return;

        if (GetAliensInJumpZoneCount() > 2)
        {
            _animator.SetTrigger("JumpAttack");
            _countFistAttack = 0;
            return;
        }

        if(_countFistAttack > 2)
        {
            _animator.SetTrigger("BeakAttack");
            _countFistAttack = 0;
            return;
        }

        _animator.SetTrigger("FistAttack");
        _countFistAttack++;
    }

    private int GetAliensInJumpZoneCount()
    {
        int count = 0;
        var chickenPos = _chicken.transform.position;

        foreach (var alien in _chicken.Aliens)
        {
            if (alien != null &&
                alien.IsOnGarden.CurrentValue &&
                Vector3.Distance(chickenPos, alien.transform.position) <= _jumpAoERadius)
            {
                count++;
            }
        }
        return count;
    }

    public void AttackEnd(float numberAttack)
    {
        //TODO rewrite Vector3.back is a hitPoint
        switch(numberAttack)
        {
            case 0f:
                _target?.TakeDamage(_attackFistDamage, Vector3.back);
                break;
            case 1f:
                _target?.TakeDamage(_attackBeakDamage, Vector3.back);
                break;
            case 2f:
                var chickenPos = _chicken.transform.position;
                foreach (var alien in _chicken.Aliens)
                {
                    if (alien != null &&
                        alien.IsOnGarden.CurrentValue &&
                        Vector3.Distance(chickenPos, alien.transform.position) <= _jumpAoERadius)
                    {
                        alien?.TakeDamage(_attackJumpDamage, Vector3.back);
                    }
                }
                break;
        }
    }
}