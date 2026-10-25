using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftTableConfig", menuName = "CraftingSystem/Config")]
public class CraftingTableConfig : ScriptableObject
{
    [field: SerializeField] public List<CraftProduct> Products { get; set; } = new();
}
