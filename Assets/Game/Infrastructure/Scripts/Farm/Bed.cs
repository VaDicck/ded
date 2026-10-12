using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    public IBedState CurrentState => _stateMachine.CurrentState;
    
    public bool IsInteractable => _stateMachine.CurrentState.IsInteractable;
    public bool IsUsable => _stateMachine.CurrentState.IsUsable;

    [SerializeField] private ToolType _unlockTool;
    [SerializeField] private ToolType _plowTool;
    
    private StateMachine<IBedState> _stateMachine;
    
    private void Start()
    {
        _stateMachine = new StateMachine<IBedState>(new ClosedBedState(_unlockTool));
        _stateMachine.AddState<RawBedState>(new RawBedState(_plowTool));
        _stateMachine.AddState<PlowedBedState>(new PlowedBedState());
        _stateMachine.AddState<PlantedBedState>(new PlantedBedState());
        
        Subscribe();
    }

    public void UnlockForce()
    {
        _stateMachine.SetState<RawBedState>();
    }
    
    private void Update() => _stateMachine.Tick();
    
    public UniTask Interact() => CurrentState.Interact();
    
    public UniTask<bool> Use(ToolType tool) => CurrentState.Use(tool);

    private void Subscribe()
    {
        _stateMachine.GetState<ClosedBedState>().Performed += _stateMachine.SetState<RawBedState>;
        _stateMachine.GetState<RawBedState>().Performed +=  _stateMachine.SetState<PlowedBedState>;
        _stateMachine.GetState<PlowedBedState>().Performed += () =>
        {
            var culture = _stateMachine.GetState<PlowedBedState>().Culture;
            _stateMachine.SetState<PlantedBedState>();
            _stateMachine.GetState<PlantedBedState>();
        };
        _stateMachine.GetState<PlantedBedState>().Performed += _stateMachine.SetState<RawBedState>;
    }
}
