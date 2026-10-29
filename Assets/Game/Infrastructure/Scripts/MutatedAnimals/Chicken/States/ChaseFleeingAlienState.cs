using System;
using UnityEngine;
using UnityEngine.AI;

public class ChaseFleeingAlienState : IChickenState
{
    public event Action Performed;
    
    private readonly MutatedChicken _chicken;      
    private readonly NavMeshAgent _agent;
    private readonly float _attackRange;
    private readonly float _attackDamage;
    private readonly float _attackInterval;

    private float _nextAttackTime;
    private AlienBase _target;
    
    private Animator _animator;

    private bool _isMoving;

    private int _testCount;

    public ChaseFleeingAlienState(MutatedChicken chicken, NavMeshAgent agent,
                                  float attackRange, float attackDamage, float attackInterval, Animator animator)
    {
        _chicken = chicken;
        _agent = agent;
        _attackRange = attackRange;
        _attackDamage = attackDamage;
        _attackInterval = attackInterval;
        _animator = animator;
    }

    public void Enter()
    {
        _nextAttackTime = Time.time;
        _isMoving = false;         
        ChooseClosestTarget();
    }

    public void Tick()
    {
        if (_chicken.Aliens.Count == 0)
        {
            Performed?.Invoke();             
            return;
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
        else if (Time.time >= _nextAttackTime)
        {
            Attack();
            _nextAttackTime = Time.time + _attackInterval;
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
            if (alien == null) continue;

            float dist = Vector3.Distance(_chicken.transform.position, alien.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                _target = alien;
            }
        }
    }

    private void Attack()
    {
        if (_target == null) return;
        
        if(_testCount == 3)
            _testCount = 0;
        if(_testCount == 0)
            _animator.SetTrigger("FistAttack");
        if(_testCount == 1)
            _animator.SetTrigger("BeakAttack");
        if(_testCount == 2)
            _animator.SetTrigger("JumpAttack");
        /*if (_chicken.Aliens.Count > 2)
        {
            _animator.SetTrigger("JumpAttack");
        }


        _animator.SetTrigger("FistAttack");*/
        _target.TakeDamage(_attackDamage, Vector3.back); 
        _testCount++;
    }

    public void AttackEnd(float numberAttack)
    {
        this.LogError($"number {numberAttack}");
        _target?.TakeDamage(_attackDamage, Vector3.back); 
    }
}