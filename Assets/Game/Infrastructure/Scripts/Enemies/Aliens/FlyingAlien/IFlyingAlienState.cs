using System;

public interface IFlyingAlienState : IState
{
    public event Action Performed;
}