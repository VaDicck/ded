using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class FleeingAlien : Alien<IFleeingAlienState>
{
    private bool _isSuccessDigging = false;
    private bool _isJumping = false;
    
    private BedService _bedService;
    
    public class Factory : PlaceholderFactory<FleeingAlien> { }

    [Inject]
    private void Init(BedService bedService)
    {
        _bedService = bedService;
    }
    
    private void Update()
    {
        if (_navMeshAgent.isOnOffMeshLink && !_isJumping)
        {
            StartCoroutine(JumpOverNavMeshLink());
        }
        
        if (!_isJumping)
        {
            _stateMachine.Tick();
        }
    }
    
    private IEnumerator JumpOverNavMeshLink()
    {
        _isJumping = true;
        _navMeshAgent.isStopped = true;
        
        OffMeshLinkData data = _navMeshAgent.currentOffMeshLinkData;
        Vector3 startPos = data.startPos;
        Vector3 endPos = data.endPos;
        
        float jumpHeight = 1.5f;  
        float duration = 0.8f; 
        float elapsed = 0;
        
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            
            float curveT = Mathf.Sin(t * Mathf.PI); 
            
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
            
            currentPos.y += curveT * jumpHeight;
            
            transform.position = currentPos;
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = endPos;
        
        _navMeshAgent.CompleteOffMeshLink();
        _navMeshAgent.isStopped = false;
        _isJumping = false;
    }
    

    protected override void AddStates()
    {
        _stateMachine = new StateMachine<IFleeingAlienState>(new IdleFleeingAlienState(_bedService));
        _stateMachine.AddState<RunToBedFleeingAlienState>(new RunToBedFleeingAlienState(_navMeshAgent, _bedService));
        _stateMachine.AddState<DiggingBedFleeingAlienState>(new DiggingBedFleeingAlienState(_bedService));
        _stateMachine.AddState<RunToSpawnFleeingAlienState>(new RunToSpawnFleeingAlienState(_navMeshAgent, _spawnPoint));
    }
    
    protected override void Subscribe()
    {
        _stateMachine.GetState<IdleFleeingAlienState>().BedSelected 
            += (Bed bed) =>
            {
                _animator.SetTrigger("Run");
                _stateMachine.GetState<RunToBedFleeingAlienState>().SetBed(bed);
                _stateMachine.SetState<RunToBedFleeingAlienState>();
            };

        _stateMachine.GetState<IdleFleeingAlienState>().BedNotSelected
            += () =>
            {
                _animator.SetTrigger("Run");
                _stateMachine.SetState<RunToSpawnFleeingAlienState>();
            };

        _stateMachine.GetState<RunToBedFleeingAlienState>().Performed 
            += (Bed bed) =>
            {
                _animator.SetTrigger("Digging");
                _stateMachine.GetState<DiggingBedFleeingAlienState>().SetBed(bed);
                _stateMachine.SetState<DiggingBedFleeingAlienState>();
            };

        _stateMachine.GetState<DiggingBedFleeingAlienState>().Performed
            += () =>
            {
                _stateMachine.SetState<IdleFleeingAlienState>();
            };

        _stateMachine.GetState<RunToSpawnFleeingAlienState>().Performed
            += () =>
            {
                _animator.SetTrigger("Run");
                Die();
            };

    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    private void Lose()
    {
        if (_isSuccessDigging)
        {
            Destroy(gameObject);
        }
    }
}