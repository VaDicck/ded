using System;
using UnityEngine;

[Serializable]
public class Range
{
    public int Min => _min;
    public int Max => _max;
    

    [SerializeField] private int _min;
    [SerializeField] private int _max;
    
    
    private void Validate()
    {
        if (_min > _max)
        {
            _max = _min;
        }
    }
}