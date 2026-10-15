using UnityEngine;

public abstract class CultureConfig : ScriptableObject
{
    [field: SerializeField] public Range DropAmount { get; private set; } = new();
    
    [field: SerializeField] public GameObject Prefab { get; private set; }
}