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
        
        if (IsPlantedBed(_bed.CurrentState, out PlantedBedState plantedBedState))
            plantedBedState.Culture.StartHitting();
    }

    public void StopHitting()
    {
        if (IsPlantedBed(_bed.CurrentState, out PlantedBedState plantedBedState))
            plantedBedState.Culture.StopHitting();
    }
    
    public void Tick()
    {
        if (IsPlantedBed(_bed.CurrentState, out PlantedBedState plantedBedState))
        {
            plantedBedState.Culture.Hit(Time.deltaTime);
            return;
        }
    
        Performed?.Invoke();
    }

    private bool IsPlantedBed(IBedState bedState, out PlantedBedState plantedBedState)
    {
        if (bedState is PlantedBedState)
        {
            plantedBedState = (PlantedBedState)bedState;
            return true;
        }

        plantedBedState = null;
        return false;
    }
    

    private void OnCanAttackChanged(bool CanAttack)
    {
        if(!CanAttack)
            Performed?.Invoke();
    }
}