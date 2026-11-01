using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Bed : MonoBehaviour, IInteractable
{
    public event Action UpdatePlantedStateBeds;
    public event Action Unlocked;
    public event Action Plowed;
    public event Action Planted; 
    public event Action Watered;
    public event Action Grown;
    public event Action Collected;
    
    [Header("Visual State")] 
    [SerializeField] private GameObject _closedState;
    [SerializeField] private GameObject _rawState;
    [SerializeField] private GameObject _plowedState;
    [SerializeField] private GameObject _plantedState;

    [Header("Planted State")] 
    [SerializeField] private GameObject _poured;

    public IBedState CurrentState => _stateMachine.CurrentState;

    public bool IsInteractable => _stateMachine.CurrentState.IsInteractable && _dayCicleService.IsDay;
    public bool IsUsable => _stateMachine.CurrentState.IsUsable && _dayCicleService.IsDay;
    public ToolType UseTool => _stateMachine.CurrentState.UseTool;
    

    [Header("Tools")] 
    [SerializeField] private ToolType _unlockTool;
    [SerializeField] private ToolType _plowTool;

    private StateMachine<IBedState> _stateMachine;
    
    private BedService _bedService;
    private CulturesSelection _culturesSelection; 
    private DayCycleService _dayCicleService; 
    
    [Inject]
    private void Initialize(BedService bedService, CulturesSelection culturesSelection, DayCycleService dayCycleService)
    {
        _bedService = bedService;
        _culturesSelection = culturesSelection;
        _dayCicleService = dayCycleService;
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
        _bedService.AddBed(this);
        
        _stateMachine = new StateMachine<IBedState>(new ClosedBedState(_unlockTool, _closedState));
        _stateMachine.AddState<RawBedState>(new RawBedState(_plowTool, _rawState));
        _stateMachine.AddState<PlowedBedState>(new PlowedBedState(_plowedState, _culturesSelection));
        _stateMachine.AddState<PlantedBedState>(new PlantedBedState(_plantedState, _poured));
        
        Subscribe();
        
        if (_unlockTool == ToolType.None)
            UnlockForce();
    }

    private void UnlockForce()
    {
        _stateMachine.SetState<RawBedState>();
    }
    
    private void Update() => _stateMachine.Tick();
    
    public UniTask Interact() => CurrentState.Interact();
    
    public UniTask<bool> Use(ToolType tool) => CurrentState.Use(tool);

    private void Subscribe()
    {
        _stateMachine.GetState<ClosedBedState>().Performed += () => Unlocked?.Invoke();
        _stateMachine.GetState<RawBedState>().Performed += () => Plowed?.Invoke();
        _stateMachine.GetState<PlowedBedState>().Performed += () => Planted?.Invoke();
        _stateMachine.GetState<PlantedBedState>().Watered += () => Watered?.Invoke();
        _stateMachine.GetState<PlantedBedState>().Grown += () => Grown?.Invoke();
        _stateMachine.GetState<PlantedBedState>().Collected += () => Collected?.Invoke();
        
        _stateMachine.GetState<ClosedBedState>().Performed += _stateMachine.SetState<RawBedState>;
        _stateMachine.GetState<RawBedState>().Performed +=  _stateMachine.SetState<PlowedBedState>;
        _stateMachine.GetState<PlowedBedState>().Performed += () =>
        {
            var culture = _stateMachine.GetState<PlowedBedState>().Culture;
            
            _stateMachine.GetState<PlantedBedState>().SetCulture(culture);
            
            _stateMachine.SetState<PlantedBedState>();
            
            UpdatePlantedStateBeds?.Invoke();
        };
        
        _stateMachine.GetState<PlantedBedState>().Performed += () =>
        {
            _stateMachine.SetState<RawBedState>();
            
            UpdatePlantedStateBeds?.Invoke();
        };
    }
}
