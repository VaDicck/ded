using UnityEngine;

public abstract class CultureConfig : ScriptableObject
{
    [field: SerializeField] public Range DropAmount { get; private set; } = new();
    
    [field: SerializeField] public GameObject Step1Prefab { get; private set; }
    [field: SerializeField] public GameObject Step2Prefab { get; private set; }

    [field: SerializeField] public GameObject Step3Prefab { get; private set; }
    
    [field: SerializeField] public bool IsMutable { get; private set; }
}