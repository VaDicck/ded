using System.Collections.Generic;
using UnityEngine;

public class QuotaElement : ScriptableObject
{
    [field: SerializeField] public List<SerializedPair<CultureType, int>> Cultures { get; private set; } = new();
}