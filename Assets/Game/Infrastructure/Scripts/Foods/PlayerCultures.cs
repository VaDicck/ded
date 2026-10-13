using System;
using System.Collections.Generic;

public class PlayerCultures
{
    private readonly Dictionary<CultureType, int> _foodCollection;
    private readonly int _foodCapacity;
    
    public PlayerCultures(GameConfig config)
    {
        _foodCapacity = config.FoodCapacity;
    }

    public int AddFood(Culture culture)
    {
        _foodCollection.TryAdd(culture.Type, 0);

        _foodCollection[culture.Type] += culture.Count;

        int extra = 0;

        if (_foodCollection[culture.Type] > _foodCapacity)
        {
            extra = _foodCollection[culture.Type] - _foodCapacity;
            _foodCollection[culture.Type] = _foodCapacity;
        }
        
        return culture.Count - extra;
    }

    public int Take(CultureType cultureType, int value)
    {
        if (_foodCollection.TryGetValue(cultureType, out var count) == false)
            return 0;

        if (count < value)
        {
            _foodCollection[cultureType] = 0;
            return count;
        }

        _foodCollection[cultureType] -= value;

        return value;
    }

    public int Count(CultureType cultureType)
    {
        if (_foodCollection.TryGetValue(cultureType, out var count) == false)
            return 0;

        return _foodCollection[cultureType];
    }
}
