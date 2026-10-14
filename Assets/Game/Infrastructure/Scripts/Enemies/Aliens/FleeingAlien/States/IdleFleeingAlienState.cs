using System;
using System.Collections.Generic;
using Random = System.Random;

public class IdleFleeingAlienState : IFleeingAlienState
{
    public event Action<Bed> Performed;

    private Random _random = new Random();
    
    private BedService _bedService;
    
    public IdleFleeingAlienState(BedService bedService)
    {
        _bedService = bedService;
    }
    
    public void Tick()
    {
        if (TrySelectRandomBed(out Bed targetBed))
        {
            Performed?.Invoke(targetBed);
        }
    }

    private bool TrySelectRandomBed(out Bed targetBed)
    {
        targetBed = null;
        
        List<Bed> beds = _bedService.GetBedsByState<PlantedBedState>();
    
       /*if (beds.Count < 1)
        {
            beds = _bedService.GetBedsByState<PlowedBedState>();
        }*/
    
        if (beds.Count > 0)
        {
            targetBed = beds[_random.Next(beds.Count)];
            return true;
        }
    
        return false;
    }
}