using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedPair<TKey, TValue>
{
    [field: SerializeField] public TKey Key { get; private set; }
    [field: SerializeField] public TValue Value { get; private set; }
    
    public override string ToString() => $"Key: {Key}, Value: {Value}";
}