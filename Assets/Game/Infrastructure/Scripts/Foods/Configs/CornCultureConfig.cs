using UnityEngine;

[CreateAssetMenu(fileName = "CornCultureConfig", menuName = "Configs/Cultures/Corn")]
public class CornCultureConfig : CultureConfig
{
    [field: SerializeField] public MutatedCorn MutatedPrefab { get; private set; }
}