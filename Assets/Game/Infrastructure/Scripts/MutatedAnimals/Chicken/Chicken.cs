using UnityEngine;

public class Chicken : ChickenBase
{
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