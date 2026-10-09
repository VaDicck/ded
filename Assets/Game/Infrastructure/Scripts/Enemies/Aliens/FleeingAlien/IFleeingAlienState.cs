using System;

public interface IFleeingAlienState : IState
{
    event Action Performed;
}