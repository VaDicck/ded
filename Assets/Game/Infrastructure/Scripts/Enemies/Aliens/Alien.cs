public abstract class Alien<TState> : AlienBase where TState : IState
{
    protected StateMachine<TState> _stateMachine;

    private void Start()
    {
        AddStates();
        
        Subscribe();
    }
    
    private void Update() => _stateMachine.Tick();

    protected abstract void Subscribe();
    
    protected abstract void AddStates();
}