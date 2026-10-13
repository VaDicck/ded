using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Bed : MonoBehaviour, IInteractable
{
    [Header("Visual State")] [SerializeField]
    private GameObject _closedState;
    [SerializeField] private GameObject _rawState;
    [SerializeField] private GameObject _plowedState;
    [SerializeField] private GameObject _plantedState;

    public IBedState CurrentState => _stateMachine.CurrentState;

    public bool IsInteractable => _stateMachine.CurrentState.IsInteractable;
    public bool IsUsable => _stateMachine.CurrentState.IsUsable;

    [Header("Tools")] [SerializeField] private ToolType _unlockTool;
    [SerializeField] private ToolType _plowTool;

    private StateMachine<IBedState> _stateMachine;
    
    private CarrotCulture.Factory _factory; // TODO: rewrite
    
    [Inject]
    private void Initialize(CarrotCulture.Factory factory)
    {
        _factory = factory;
    }

    private void Awake()
    {
        _closedState.gameObject.SetActive(false);
        _rawState.gameObject.SetActive(false);
        _plowedState.gameObject.SetActive(false);
        _plantedState.gameObject.SetActive(false);
    }
    
    private void Start()
    {
        _stateMachine = new StateMachine<IBedState>(new ClosedBedState(_unlockTool, _closedState));
        _stateMachine.AddState<RawBedState>(new RawBedState(_plowTool, _rawState));
        _stateMachine.AddState<PlowedBedState>(new PlowedBedState(_plowedState, _factory));
        _stateMachine.AddState<PlantedBedState>(new PlantedBedState(_plantedState));
        
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
            
            //TODO: rewrite
            _stateMachine.GetState<PlantedBedState>().SetCulture(culture);
            
            _stateMachine.SetState<PlantedBedState>();
            _stateMachine.GetState<PlantedBedState>();
        };
        _stateMachine.GetState<PlantedBedState>().Performed += _stateMachine.SetState<RawBedState>;
    }
}
