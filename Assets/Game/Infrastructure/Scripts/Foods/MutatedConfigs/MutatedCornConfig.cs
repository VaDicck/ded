using UnityEngine;

[CreateAssetMenu(fileName = "MutatedCornConfig", menuName = "Configs/MutatedCultures/Corn")]
public class MutatedCornConfig : MutatedCultureConfig
{
    [field: SerializeField] public Range CountMetal { get; private set; } = new();
    [field: SerializeField] public Range CountScrap { get; private set; } = new();
    [field: SerializeField] public Range CountWood { get; private set; } = new();
}
