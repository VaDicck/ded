using UnityEngine;

public abstract class CultureConfig : ScriptableObject
{
    [field: SerializeField] public Range DropAmount { get; private set; } = new();
    
    [field: SerializeField] public GameObject Step1Prefab { get; private set; }
    [field: SerializeField] public GameObject Step2Prefab { get; private set; }

    [field: SerializeField] public GameObject Step3Prefab { get; private set; }
    
    [field: SerializeField] public float Health { get; private set; }
    [field: SerializeField] public float HealthMutated { get; private set; }
}