using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuotaElement
{
    public event Action<CultureType, int> Added;
    public event Action Reached;
    
    public bool IsReached { get; private set; }
    public Quota.Element Element { get; private set; }
    
    [field: SerializeField] public List<SerializedPair<CultureType, int>> Expected { get; private set; } = new();
    [field: SerializeField] public QuotaReward Reward { get; private set; } = new();
    
    public IReadOnlyDictionary<CultureType, int> Actual => _cultures;
    
    private Dictionary<CultureType, int> _cultures;
    
    public QuotaElement(Quota.Element element)
    {
        Element = element;
    }

    public void Refresh()
    {
        _cultures = new();

        foreach (var pair in Expected)
        {
            _cultures.TryAdd(pair.Key, 0);
        }
    }

    public void ApplyReward(PlayerIngredients playerIngredients)
    {
        foreach (var ingredient in Reward.Ingredients)
        {
            playerIngredients.Add(ingredient.Key, ingredient.Value);
        }
    }
    
    public int Fill(CultureType culture, int value)
    {
        if (Actual.ContainsKey(culture) == false)
            return 0;
        
        _cultures[culture] += value;

        int extra = 0;

        if (_cultures[culture] > Expected.Find(x => x.Key == culture).Value)
            extra = _cultures[culture] - Expected.Find(x => x.Key == culture).Value;
        
        Added?.Invoke(culture, _cultures[culture]);
        DetermineReached();
        
        this.Log($"Quota {Element} added for culture {culture} with value {value}");
        
        return value - extra;
    }
    
    public bool IsEmpty => Expected.Count == 0;

    public override string ToString()
    {
        string res = "{";

        foreach (var pair in Expected)
        {
            res += $"{pair.Key}: {pair.Value};";
        }

        res += "}";

        return res;
    }

    private void DetermineReached()
    {
        foreach (var pair in Expected)
        {
            if (Actual[pair.Key] < pair.Value)
                return;
        }

        IsReached = true;
        Reached?.Invoke();
    }
}