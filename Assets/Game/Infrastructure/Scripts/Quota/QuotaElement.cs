using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[Serializable]
public class QuotaElement
{
    public event Action<CultureType, int> Added;
    public event Action Reached;
    
    [field: SerializeField] public List<SerializedPair<CultureType, int>> Expected { get; private set; } = new();

    public Dictionary<CultureType, int> Actual => _cultures;
    
    private readonly Dictionary<CultureType, int> _cultures;
    
    public QuotaElement()
    {
        _cultures = new();

        foreach (var pair in Expected)
        {
            _cultures.TryAdd(pair.Key, 0);
        }
    }

    public int Fill(CultureType culture, int value)
    {
        if (Actual.ContainsKey(culture) == false)
            return 0;
        
        Actual[culture] += value;

        int extra = 0;

        if (Actual[culture] > Expected.Find(x => x.Key == culture).Value)
            extra = Actual[culture] - Expected.Find(x => x.Key == culture).Value;
        
        Added?.Invoke(culture, Actual[culture]);
        DetermineReached();

        return value - extra;
    }
    
    public bool IsEmpty => Expected.Count == 0;
    
    public override string ToString() => Expected.ToString();

    private void DetermineReached()
    {
        foreach (var pair in Expected)
        {
            if (Actual[pair.Key] < pair.Value)
                return;
        }
        
        Reached?.Invoke();
    }
}