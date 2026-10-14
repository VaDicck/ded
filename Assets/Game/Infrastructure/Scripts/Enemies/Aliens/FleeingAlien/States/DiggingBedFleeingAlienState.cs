using System;
using UnityEngine;

public class DiggingBedFleeingAlienState : IFleeingAlienState
{
    public event Action<Bed> Performed;

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
        Performed?.Invoke(_bed);
    }
}