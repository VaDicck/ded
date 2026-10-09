using UnityEngine;
using Zenject;

public class FleeingAlien : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health = 50f;
    private Transform _bed;
    private Transform _spawnPoint;
    
    private StateMachine<IFleeingAlienState> _stateMachine;
    
    private void Start()
    {
        _stateMachine = new StateMachine<IFleeingAlienState>(new IdleFleeingAlienState());
        _stateMachine.AddState<RunFleeingAlienState>(new RunFleeingAlienState(transform, 1));
        _stateMachine.AddState<DiggingBedFleeingAlienState>(new DiggingBedFleeingAlienState());
        
        Subscribe();
    }
    
    private void Update() => _stateMachine.Tick();
    
    public void SetBed(Transform bed)
    {
        _bed = bed;
    }
    
    public void SetSpawnPoint(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
    }
    
    public void TakeDamage(float damage)
    {
        _health -= damage;
        
        if(_health <= 0f)
            Die();
    }
    
    public class Factory : PlaceholderFactory<FleeingAlien>
    {
        
    }
    
    private void Subscribe()
    {
        _stateMachine.GetState<IdleFleeingAlienState>().Performed 
            += () => RunTo(_bed);
        _stateMachine.GetState<RunFleeingAlienState>().Performed 
            +=  _stateMachine.SetState<DiggingBedFleeingAlienState>;
        _stateMachine.GetState<DiggingBedFleeingAlienState>().Performed 
            += () => RunTo(_spawnPoint);
        _stateMachine.GetState<RunFleeingAlienState>().Performed 
            +=  Lose;
    }

    private void RunTo(Transform target)
    {
        _stateMachine.SetState<RunFleeingAlienState>();
        
        _stateMachine.GetState<RunFleeingAlienState>().SetDestination(target);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void Lose()
    {
        //Destroy(gameObject);
    }
}