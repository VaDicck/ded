using UnityEngine;

public abstract class QuotaExtraReward : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    
    public abstract void GetReward();
}