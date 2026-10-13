using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class FleeingAlien : Alien<IFleeingAlienState>
{
    private bool _isSuccessDigging = false;
    private bool _isJumping = false;
    
    public class Factory : PlaceholderFactory<FleeingAlien>
    {
        
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
        _stateMachine = new StateMachine<IFleeingAlienState>(new IdleFleeingAlienState());
        _stateMachine.AddState<RunFleeingAlienState>(new RunFleeingAlienState(_navMeshAgent));
        _stateMachine.AddState<DiggingBedFleeingAlienState>(new DiggingBedFleeingAlienState());
    }
    
    protected override void Subscribe()
    {
        _stateMachine.GetState<IdleFleeingAlienState>().Performed 
            += () => RunTo(_bed);
        _stateMachine.GetState<RunFleeingAlienState>().Performed 
            +=  _stateMachine.SetState<DiggingBedFleeingAlienState>;
        _stateMachine.GetState<DiggingBedFleeingAlienState>().Performed 
            += () =>
            {
                _isSuccessDigging = true;
                RunTo(_spawnPoint);
            };
        _stateMachine.GetState<RunFleeingAlienState>().Performed 
            +=  Lose;
    }

    private void RunTo(Transform target)
    {
        _stateMachine.GetState<RunFleeingAlienState>().SetDestination(target);
        
        _stateMachine.SetState<RunFleeingAlienState>();
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