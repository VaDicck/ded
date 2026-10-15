using System;
using UnityEngine;

public class DiggingBedFleeingAlienState : IFleeingAlienState
{
    public event Action Performed;

    private Bed _bed;
    private BedService _bedService;
    
    public DiggingBedFleeingAlienState(BedService bedService)
    {
        _bedService = bedService;
    }
    
    public void SetBed(Bed bed)
    {
        _bed  = bed;
    }
    
    public void Tick()
    {
        if (_bedService.CanAttackBeds)
        {
            if (_bed.CurrentState is PlantedBedState)
            {
                PlantedBedState plantedBedState = (PlantedBedState)_bed.CurrentState;

                plantedBedState.Culture.Hit(Time.deltaTime);
                
                return;
            }
            
            Performed?.Invoke();
        }
        
        Performed?.Invoke();
    }
}