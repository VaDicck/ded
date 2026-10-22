using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class FleeingAlien : Alien<IFleeingAlienState>
{
    [SerializeField] private GameObject _brokenPartsPrefab;
    [SerializeField] private GameObject _alienModel;
    
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

            Vector3 direction = (endPos - startPos).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

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
            += bed =>
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
            += bed =>
            {
                _navMeshAgent.isStopped = true;
                _navMeshAgent.updateRotation = false;
                
                StartCoroutine(FaceTarget(bed.transform.position));
                
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
    
    private IEnumerator FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0; // Чтобы не наклонялся
    
        if (lookPos != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookPos);
            float time = 0;
        
            // Поворачиваемся плавно в течение 0.3 секунд
            while (time < 0.3f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, time / 0.3f);
                time += Time.deltaTime;
                yield return null;
            }
            transform.rotation = targetRotation;
        }
    }

    protected override void Die()
    {
        if (_stateMachine.CurrentState is DiggingBedFleeingAlienState diggingState)
            diggingState.StopHitting();

        //_alienModel.SetActive(false);
        
        GameObject parts = Instantiate(_brokenPartsPrefab, transform.position, transform.rotation);

        /*if (parts.TryGetComponent(out AlienBroken alienBroken))
        {
            alienBroken.Destroyed += () => Destroy(gameObject);
            return;
        }*/
        
        Destroy(gameObject);
    }
}