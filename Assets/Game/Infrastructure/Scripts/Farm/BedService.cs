using System.Collections.Generic;
using System.Linq;

public class BedService
{
    public List<Bed> Beds => _beds;

    public bool CanAttackBeds
    {
        get
        {
            //this.Log($"{_countPlantedBedsOnStartNight} {GetBedsByState<PlowedBedState>().Count} {_procentSaveBed}");
            if (_countPlantedBedsOnStartNight == 0)
                return false;
            
            int bedsToProtect = GetBedsByState<PlantedBedState>().Count;
            
            return bedsToProtect > _countPlantedBedsOnStartNight * _procentSaveBed;
        }
    }

    private readonly List<Bed> _beds = new();
    private DayCycleService _dayCycle;

    private float _procentSaveBed = 0.6f;

    private int _countPlantedBedsOnStartNight;
    
    public List<Bed> GetBedsByState<T>() where T : IBedState =>
        _beds.Where(bed => bed.CurrentState is T).ToList();

    public BedService(DayCycleService dayCycleService)
    {
        _dayCycle = dayCycleService;
        
        _dayCycle.NightStarted += NightStarted;
        
        _dayCycle.NightStarted += BlockBeds;
        _dayCycle.DayStarted += UnblockBeds;
    }
    
    public void AddBed(Bed bed)
    {
        _beds.Add(bed);
    }

    private void NightStarted()
    {
        _countPlantedBedsOnStartNight = GetBedsByState<PlantedBedState>().Count;
    }
    
    private void BlockBeds()
    {
        
    }

    private void UnblockBeds()
    {
        
    }
}