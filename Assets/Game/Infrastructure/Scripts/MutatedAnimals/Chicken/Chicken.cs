using Cysharp.Threading.Tasks;

public class Chicken : ChickenBase, IInteractable
{
    public bool IsInteractable { get; }
    
    public bool IsUsable { get; }
    
    public UniTask Interact()
    {
        throw new System.NotImplementedException();
    }
    
    protected override void OnDayStart()
    {
        //TODO KUKAREKU
        gameObject.SetActive(true);
        
        WarpToSpawnPoint();
    }

    protected override void OnDayEnd()
    {
        gameObject.SetActive(false);
    }

    protected override void Subscribe()
    {
        _stateMachine.GetState<IdleChickenState>().Performed += () =>
        {
            _stateMachine.SetState<WanderChickenState>();
        };
        
        _stateMachine.GetState<PeckChickenState>().Performed += () =>
        {
            _stateMachine.SetState<WanderChickenState>();
        };
    }
}