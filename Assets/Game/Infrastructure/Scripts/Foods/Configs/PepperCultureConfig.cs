using UnityEngine;

[CreateAssetMenu(fileName = "PepperCultureConfig", menuName = "Configs/Cultures/Pepper")]
public class PepperCultureConfig : CultureConfig 
{
    [field: SerializeField] public MutatedPepper MutatedPrefab { get; private set; }
}