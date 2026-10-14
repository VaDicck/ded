using System.Collections.Generic;
using System.Linq;

public class BedService
{
    public List<Bed> Beds => _beds;
    
    private readonly List<Bed> _beds = new();

    public List<Bed> GetBedsByState<T>() where T : IBedState =>
        _beds.Where(bed => bed.CurrentState is T).ToList();
    
    public void AddBed(Bed bed)
    {
        _beds.Add(bed);
    }
}