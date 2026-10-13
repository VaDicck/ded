using System;
using UnityEngine;

public abstract class Culture : ScriptableObject
{
    [field: SerializeField] public Range Range { get; private set; } = new();
    
    public abstract CultureType Type { get; }
    
    public int Count => _count;
    
    protected int _count;
}