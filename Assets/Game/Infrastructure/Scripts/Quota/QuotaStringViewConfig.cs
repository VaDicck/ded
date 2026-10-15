using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuotaStringView", menuName = "Configs/Quotas/View/QuotaStringViewConfig")]
public class QuotaStringViewConfig : ScriptableObject
{
    [field: SerializeField] public List<SerializedPair<CultureType, string>> Corresponds { get; private set; } = new();
}