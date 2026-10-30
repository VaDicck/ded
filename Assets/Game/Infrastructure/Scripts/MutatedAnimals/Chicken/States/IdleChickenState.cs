using System;
using UnityEngine;

public class IdleChickenState : IChickenState
{
    public event Action Performed;

    private Animator _animator;

    public IdleChickenState(Animator animator)
    {
        _animator = animator;
    }

    public void Enter()
    {
        _animator.SetTrigger("Idle");
    }

    public void Tick()
    {
        Performed?.Invoke();
    }
}
