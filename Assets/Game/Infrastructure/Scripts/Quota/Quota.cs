using UnityEngine;

public class Quota : ScriptableObject
{
    [field: SerializeField] public QuotaElement Main { get; private set; } = new();
    
    [field: SerializeField] public QuotaElement Sub1 { get; private set; } = new();
    
    [field: SerializeField] public QuotaElement Sub2 { get; private set; } = new();
}