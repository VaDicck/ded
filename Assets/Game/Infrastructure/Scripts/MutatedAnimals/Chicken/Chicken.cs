using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Chicken : ChickenBase, IInteractable
{
    public class Factory : PlaceholderFactory<Chicken> { }
    
    public bool IsInteractable => false;

    public bool IsUsable => !_isMutated;
    
    public ToolType UseTool => ToolType.Mutagen;
    
    private bool _isMutated = false;
    
    [SerializeField] private AudioClip _dieSound;
    [SerializeField] private AudioClip _peckSound;
    
    [Inject] private MutatedChicken.Factory _mutatedChickenFactory;
    [Inject] private PlayerMutagens _playerMutagens;
    
    public UniTask Interact()
    {
        return UniTask.CompletedTask;
    }

    public UniTask<bool> Use(ToolType tool)
    {
        if (tool != ToolType.Mutagen)
            return UniTask.FromResult(false);

        if(!_playerMutagens.Use())
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
        _stateMachine = new StateMachine<IChickenState>(new IdleChickenState(_animator));
        _stateMachine.AddState(new WanderChickenState(transform, _navMeshAgent, _wanderRadius));
        _stateMachine.AddState(new PeckChickenState(_navMeshAgent, _peckDuration));
    }
    
    protected override void OnDayStart()
    {
        //TODO KUKAREKU
        gameObject.SetActive(true);

        Walk();
        
        WarpToSpawnPoint();
    }

    protected override void OnDayEnd()
    {
        if (_isMutated)
        {
            Mutate();
            
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

    private void UseMutagen()
    {
        _animator.SetTrigger("Die");
        _audio.Stop();
        _audio.PlayOneShot(_dieSound);
        
        _isMutated = true;
        _navMeshAgent.isStopped = true;

        Unsubscribe();
    }

    private void Mutate()
    {
        MutatedChicken mutatedChicken = _mutatedChickenFactory.Create();
            
        mutatedChicken.SetSpawnPoint(transform);
            
        mutatedChicken.WarpTo(transform.position);
            
        Destroy(gameObject);
    }
    
    protected override void OnPeck()
    {
        _audio.clip = _peckSound;
        _audio.Play();
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        Unsubscribe();
    }
}