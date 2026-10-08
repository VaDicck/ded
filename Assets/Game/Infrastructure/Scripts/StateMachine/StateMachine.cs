using System;
using System.Collections.Generic;

public class StateMachine<T> where T : IState
{
    public T CurrentState => _currentState;
    
    private readonly Dictionary<Type,  T> _states = new();
    private T _currentState;

    public StateMachine(T defaultState)
    {
        _states.Add(defaultState.GetType(), defaultState);
        _currentState = defaultState;
        defaultState.Enter();
    }
    
    public void Tick() => _currentState?.Tick();

    public void AddState<TK>(TK state) where TK : T
    {
        if (_states.ContainsKey(typeof(TK)))
            throw new ArgumentException("State already exists");
        
        _states.Add(typeof(TK), state);
    }
    
    public void RemoveState<TK>() where TK : T
    {
        if (!_states.ContainsKey(typeof(TK)))
            throw new ArgumentException("State does not exist");
        
        _states.Remove(typeof(TK));
    }

    public void SetState<TK>() where TK : T
    {
        if (!_states.ContainsKey(typeof(TK)))
            throw new ArgumentException("State does not exist");
        
        _currentState?.Exit();
        _currentState = _states[typeof(TK)];
        _currentState.Enter();
    }

    public TK GetState<TK>() where TK : T
    {
        if (!_states.ContainsKey(typeof(TK)))
            throw new ArgumentException("State does not exist");

        return (TK) _states[typeof(TK)];
    }
}
