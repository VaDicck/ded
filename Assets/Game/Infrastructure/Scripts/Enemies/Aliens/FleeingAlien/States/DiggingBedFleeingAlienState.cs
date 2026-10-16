using System;
using UnityEngine;
using R3;

public class DiggingBedFleeingAlienState : IFleeingAlienState
{
    public event Action Performed;

    private Bed _bed;
    private BedService _bedService;
    
    public DiggingBedFleeingAlienState(BedService bedService)
    {
        _bedService = bedService;

        _bedService.CanAttackBeds.Subscribe(OnCanAttackChanged);
    }
    
    public void SetBed(Bed bed)
    {
        _bed  = bed;
    }
    
    public void Tick()
    {
        if (_bed.CurrentState is PlantedBedState)
        {
            PlantedBedState plantedBedState = (PlantedBedState)_bed.CurrentState;

            plantedBedState.Culture.Hit(Time.deltaTime);
                
            return;
        }
            
        Performed?.Invoke();
    }

    private void OnCanAttackChanged(bool CanAttack)
    {
        if(!CanAttack)
            Performed?.Invoke();
    }
}