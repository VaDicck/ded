using Cysharp.Threading.Tasks;
using UnityEngine;

public class Chicken : ChickenBase, IInteractable
{
    [SerializeField] GameObject _mutatedChickenPrefab;
    
    public bool IsInteractable => false;

    public bool IsUsable => true;
    
    private bool _isMutated = false;
    
    public UniTask Interact()
    {
        return UniTask.CompletedTask;
    }

    public UniTask<bool> Use(ToolType tool)
    {
        if (tool != ToolType.Mutagen)
            return UniTask.FromResult(false);
        
        //TODO take mutagen
        _isMutated = true;
        return UniTask.FromResult(true);
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
            GameObject mutatedChickenGameObject = Instantiate(
                _mutatedChickenPrefab, 
                transform.position, 
                transform.rotation);
            
            MutatedChicken mutatedChicken = mutatedChickenGameObject.GetComponent<MutatedChicken>();

            mutatedChicken.WarpToSpawnPoint();
            
            Destroy(gameObject);
            
            return;
        }

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