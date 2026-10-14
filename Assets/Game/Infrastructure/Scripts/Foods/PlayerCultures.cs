using System;
using System.Collections.Generic;

public class PlayerCultures
{
    public event Action<CultureType> CultureEmpty;
    public event Action<CultureType, int> CultureCountChanged;
    
    public IReadOnlyDictionary<CultureType, int> Cultures => _foodCollection;
    
    private readonly Dictionary<CultureType, int> _foodCollection = new();
    private readonly int _foodCapacity;
    
    public PlayerCultures(GameConfig config)
    {
        _foodCapacity = config.FoodCapacity;
    }

    public int AddCultureValue(Culture culture)
    {
        CultureType cultureType =culture.Type;
        
        _foodCollection.TryAdd(cultureType, 0);

        _foodCollection[cultureType] += culture.Count;

        int extra = 0;

        if (_foodCollection[cultureType] > _foodCapacity)
        {
            extra = _foodCollection[cultureType] - _foodCapacity;
            _foodCollection[cultureType] = _foodCapacity;
        }
                
        CultureCountChanged?.Invoke(cultureType, _foodCollection[cultureType]);
        
        this.Log($"Added {culture.Count - extra} of type: {cultureType}");
        
        return culture.Count - extra;
    }

    public int Take(CultureType cultureType, int value)
    {
        if (_foodCollection.TryGetValue(cultureType, out var count) == false)
            return 0;

        if (count < value)
        {
            _foodCollection[cultureType] = 0;
            
            CultureEmpty?.Invoke(cultureType);
            
            return count;
        }
        
        _foodCollection[cultureType] -= value;
        
        CultureCountChanged?.Invoke(cultureType, _foodCollection[cultureType]);
        
        this.Log($"taken {value} of type: {cultureType}");
        
        return value;
    }

    public int Count(CultureType cultureType)
    {
        if (_foodCollection.TryGetValue(cultureType, out var count) == false)
            return 0;

        return _foodCollection[cultureType];
    }
}
