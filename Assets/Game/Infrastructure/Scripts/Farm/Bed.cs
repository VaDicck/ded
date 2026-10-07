using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    public IBedState CurrentState => _stateMachine.CurrentState;

    [SerializeField] private ToolType _unlockTool;
    
    private StateMachine<IBedState> _stateMachine;
    
    private void Start()
    {
        _stateMachine = new StateMachine<IBedState>(new ClosedBedState(_unlockTool));
    }

    private void Update() => _stateMachine.Tick();
    
    public void Interact() => CurrentState?.Interact();
    
    public UniTask<bool> Use(ToolType tool) => CurrentState.Use(tool);
}
