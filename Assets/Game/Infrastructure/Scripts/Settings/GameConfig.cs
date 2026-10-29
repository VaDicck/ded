using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    [field: SerializeField] public float DayLength { get; private set; }
    [field: SerializeField] public int FoodCapacity { get; private set; }
    [field: SerializeField] public QuotasConfig Quotas { get; private set; }
    [field: SerializeField] public CraftingTableConfig CraftingTable { get; private set; }
    [field: SerializeField] public List<CultureType> UnlockedCultures { get; private set; } = new();
}
