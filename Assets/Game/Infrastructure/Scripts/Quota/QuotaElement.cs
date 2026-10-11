using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuotaElement
{
    [field: SerializeField] public List<SerializedPair<CultureType, int>> Cultures { get; private set; } = new();
}