using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CulturesConfig
{
    [field: SerializeField] public List<SerializedPair<CultureType, Sprite>> Sprites { get; private set; } = new();
    [field: SerializeField] public List<SerializedPair<CultureType, string>> Names { get; private set; } = new();
}