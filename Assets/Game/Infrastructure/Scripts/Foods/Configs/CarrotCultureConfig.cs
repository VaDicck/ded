using UnityEngine;

[CreateAssetMenu(fileName = "CarrotCultureConfig", menuName = "Configs/Cultures/Carrot")]
public class CarrotCultureConfig : CultureConfig
{
    [field: SerializeField] public MutatedCarrot MutatedPrefab { get; private set; }
}