using System.Collections.Generic;
using System.Linq;
using R3;

public class BedService
{
    public List<Bed> Beds => _beds;

    public ReadOnlyReactiveProperty<bool> CanAttackBeds => _canAttackBeds;
    
    private readonly List<Bed> _beds = new();
    private DayCycleService _dayCycle;

    private float _procentSaveBed = 0.6f;

    private int _countPlantedBedsOnStartNight;
    
    private readonly ReactiveProperty<int> _countPlantedBeds = new(0);
    private readonly ReactiveProperty<bool> _canAttackBeds = new(false);
    
    public List<Bed> GetBedsByState<T>() where T : IBedState =>
        _beds.Where(bed => bed.CurrentState is T).ToList();

    public BedService(DayCycleService dayCycleService)
    {
        _dayCycle = dayCycleService;
        
        _dayCycle.NightStarted += NightStarted;
        
        _dayCycle.NightStarted += BlockBeds;
        _dayCycle.DayStarted += UnblockBeds;

        _countPlantedBeds
            .Select(currentCount =>
            {
                if (_countPlantedBedsOnStartNight == 0)
                    return false;

                return currentCount > _countPlantedBedsOnStartNight * _procentSaveBed;
            })
            .Subscribe(value => _canAttackBeds.Value = value);
    }
    
    public void AddBed(Bed bed)
    {
        _beds.Add(bed);

        bed.UpdatePlantedStateBeds += UpdatePlantedBeds;
    }

    private void NightStarted()
    {
        _countPlantedBedsOnStartNight = GetBedsByState<PlantedBedState>().Count;
        
        if (_countPlantedBedsOnStartNight == 0)
        {
            _canAttackBeds.Value = false;
            return;
        }

        _canAttackBeds.Value = _countPlantedBeds.CurrentValue > _countPlantedBedsOnStartNight * _procentSaveBed;
    }
    
    private void BlockBeds()
    {
        
    }

    private void UnblockBeds()
    {
        
    }

    private void UpdatePlantedBeds()
    {
        _countPlantedBeds.Value = GetBedsByState<PlantedBedState>().Count;
    }
}