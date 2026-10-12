using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuotasConfig", menuName = "Configs/Quotas/QuotasConfig")]
public class QuotasConfig : ScriptableObject
{
    [field: SerializeField] public List<Quota> Quotas { get; private set; } = new();
}