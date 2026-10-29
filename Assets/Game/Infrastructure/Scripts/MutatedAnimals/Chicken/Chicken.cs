using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Chicken : ChickenBase, IInteractable
{
    public class Factory : PlaceholderFactory<Chicken> { }
    
    public bool IsInteractable => false;

    public bool IsUsable => !_isMutated;
    
    private bool _isMutated = false;
    
    [Inject] private MutatedChicken.Factory _mutatedChickenFactory;
    
    public UniTask Interact()
    {
        return UniTask.CompletedTask;
    }

    public UniTask<bool> Use(ToolType tool)
    {
        if (tool != ToolType.Mutagen)
            return UniTask.FromResult(false);

        UseMutagen();
        return UniTask.FromResult(true);
    }
    
    protected void Update()
    {
        _stateMachine.Tick();
        
        if (!_dayCycleService.IsDay ||  _isMutated)
            return;
        
        PeckCheck();
    }
    
    protected override void AddStates()
    {
        _stateMachine = new StateMachine<IChickenState>(new IdleChickenState());
        _stateMachine.AddState(new WanderChickenState(transform, _navMeshAgent, _wanderRadius));
        _stateMachine.AddState(new PeckChickenState(_navMeshAgent, _peckDuration));
    }
    
    protected override void OnDayStart()
    {
        //TODO KUKAREKU
        gameObject.SetActive(true);
        
        WarpToSpawnPoint();
    }

    protected override void OnDayEnd()
    {
        if (_isMutated)
        {
            MutatedChicken mutatedChicken = _mutatedChickenFactory.Create();
            
            mutatedChicken.SetSpawnPoint(transform);
            
            mutatedChicken.WarpTo(transform.position);
            
            Destroy(gameObject);
            
            return;
        }

        gameObject.SetActive(false);
    }

    protected override void Subscribe()
    {
        _stateMachine.GetState<IdleChickenState>().Performed += Walk;

        _stateMachine.GetState<PeckChickenState>().Performed += Walk;
    }

    private void Unsubscribe()
    {
        _stateMachine.GetState<IdleChickenState>().Performed -= Walk;

        _stateMachine.GetState<PeckChickenState>().Performed -= Walk;
    }

    [ContextMenu("Use Mutagen")]
    private void UseMutagen()
    {
        //TODO take mutagen
        _isMutated = true;
        this.Log("use mutagen");
        _navMeshAgent.isStopped = true;

        Unsubscribe();
    }
}