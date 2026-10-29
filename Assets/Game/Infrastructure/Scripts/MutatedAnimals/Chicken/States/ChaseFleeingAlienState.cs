using UnityEngine;
using UnityEngine.AI;

public class ChaseFleeingAlienState : IChickenState
{
    private readonly MutatedChicken _chicken;      
    private readonly NavMeshAgent _agent;
    private readonly float _attackRange;
    private readonly float _attackDamage;
    private readonly float _attackInterval;

    private float _nextAttackTime;
    private AlienBase _target;                      
    private Animator _animator;

    private int _countAttack = 0;

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
        ChooseClosestTarget();
    }

    public void Tick()
    {
        if (_chicken.Aliens == null || _chicken.Aliens.Count == 0)
        {
            return;
        }

        if (_target == null || !_chicken.Aliens.Contains(_target))
        {
            ChooseClosestTarget();
            if (_target == null) return;
        }

        _animator.SetTrigger("Walk");
        _agent.SetDestination(_target.transform.position);

        float distance = Vector3.Distance(_chicken.transform.position, _target.transform.position);

        if (distance <= _attackRange && Time.time >= _nextAttackTime)
        {
            Attack();
            _nextAttackTime = Time.time + _attackInterval;
        }
    }

    public void Exit()
    {
        _agent.ResetPath();   
        _target = null;
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

        _countAttack++;
        if(_countAttack == 3)
        {

        }
        _animator.SetTrigger("FistAttack");
        _target.TakeDamage(_attackDamage, Vector3.back); 
    }
}