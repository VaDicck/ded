using UnityEngine;

public class MutatedCultureConfig : ScriptableObject
{
    [field: SerializeField] public GameObject MutatedPrefab { get; private set; }

    [field: SerializeField] public float HealthMutated { get; private set; }
}
