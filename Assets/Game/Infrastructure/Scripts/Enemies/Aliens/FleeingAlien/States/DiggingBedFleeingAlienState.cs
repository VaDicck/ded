using System;
using UnityEngine;

public class DiggingBedFleeingAlienState : IFleeingAlienState
{
    public event Action Performed;

    private Bed _bed;
    
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
        }

        if(_bed.CurrentState is RawBedState)
        {
            Performed?.Invoke();
        }
    }
}