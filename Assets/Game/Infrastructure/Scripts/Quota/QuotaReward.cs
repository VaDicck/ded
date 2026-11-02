using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public class QuotaReward
{
    public QuotaExtraReward ExtraReward => _extraReward;
    [field: SerializeField] public List<SerializedPair<ResourceType, int>> Ingredients { get; private set; } = new();
    [SerializeField] private QuotaExtraReward _extraReward;

    public void GetReward()
    {
        _extraReward?.GetReward();
    }

    public void Initialize(DiContainer container)
    {
        if (_extraReward != null)
            container.Inject(_extraReward);
    }
}