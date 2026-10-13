using UnityEngine;

[CreateAssetMenu(fileName = "CultureConfig", menuName = "Configs/Cultures/CultureConfig")]
public class CultureConfig : ScriptableObject
{
    [field: SerializeField] public Range DropAmount { get; private set; } = new();
    
    [field: SerializeField] public GameObject Prefab { get; private set; }
}