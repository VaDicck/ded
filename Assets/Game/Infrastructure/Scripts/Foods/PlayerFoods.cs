using System;
using System.Collections.Generic;

public class PlayerFoods
{
    private readonly Dictionary<Type, int> _foodCollection;
    private readonly int _foodCapacity;
    
    public PlayerFoods(GameConfig config)
    {
        _foodCapacity = config.FoodCapacity;
    }

    public int AddFood<T>(T food) where T : Food
    {
        if (_foodCollection.ContainsKey(typeof(T)) == false)
            _foodCollection.Add(typeof(T), 0);

        _foodCollection[typeof(T)] += food.Count;

        int extra = 0;

        if (_foodCollection[typeof(T)] > _foodCapacity)
        {
            extra = _foodCollection[typeof(T)] - _foodCapacity;
            _foodCollection[typeof(T)] = _foodCapacity;
        }
        
        return food.Count - extra;
    }

    public int Take<T>(int value) where T : Food
    {
        if (_foodCollection.ContainsKey(typeof(T)) == false)
            return 0;

        int count = _foodCollection[typeof(T)];

        if (count < value)
        {
            _foodCollection[typeof(T)] = 0;
            return count;
        }

        _foodCollection[typeof(T)] -= value;

        return value;
    }
}
