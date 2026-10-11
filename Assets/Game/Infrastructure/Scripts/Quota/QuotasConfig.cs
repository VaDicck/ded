using System.Collections.Generic;
using UnityEngine;

public class QuotasConfig : ScriptableObject
{
    [field: SerializeField] public List<Quota>  Quotas { get; private set; } = new();
}