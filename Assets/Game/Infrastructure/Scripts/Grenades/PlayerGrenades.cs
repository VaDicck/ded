using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerGrenades
{
    public event Action<ToolType> Added;
    public event Action<ToolType, int> Used;
    public static readonly IReadOnlyList<ToolType> AllowedGrenades = new List<ToolType>() { ToolType.TomatoGrenade };
    
    private Dictionary<ToolType, int> _grenades = new();

    public void Add(ToolType grenade)
    {
        if (!AllowedGrenades.Contains(grenade))
            return;
        
        if (!_grenades.TryAdd(grenade, 1))
            _grenades[grenade]++;
        
        Added?.Invoke(grenade);
    }

    public bool Contains(ToolType grenade)
    {
        return _grenades.ContainsKey(grenade);
    }

    public int Count(ToolType grenade)
    {
        return _grenades.GetValueOrDefault(grenade, 0);
    }
    
    public bool Use(ToolType grenade)
    {
        if (!AllowedGrenades.Contains(grenade))
            return false;

        if (!_grenades.TryGetValue(grenade, out var count))
            return false;

        if (count < 1)
            return false;
            
        _grenades[grenade]--;
        Used?.Invoke(grenade, _grenades[grenade]);
        return true;
    }
}