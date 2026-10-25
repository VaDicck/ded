using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuotaReward
{
    [field: SerializeField] public List<SerializedPair<IngredientType, int>> Ingredients { get; private set; } = new();
}