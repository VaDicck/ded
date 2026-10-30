using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourcesConfig
{
    [field: SerializeField] public List<SerializedPair<ResourceType, Sprite>> Sprites { get; private set; } = new();
    [field: SerializeField] public List<SerializedPair<ResourceType, string>> Names { get; private set; } = new();
}