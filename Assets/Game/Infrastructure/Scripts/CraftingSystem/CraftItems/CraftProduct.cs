using System.Collections.Generic;
using UnityEngine;

public abstract class CraftProduct : ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public int CraftAmount { get; private set; }

    [field: SerializeField] public List<SerializedPair<CraftIngredient, int>> Ingredients { get; private set; } = new();
    
    public bool IsEnoughToCraft()
    {
        foreach (var pair in Ingredients)
        {
            if (pair.Key.IsEnough(pair.Value) == false)
                return false;
        }

        return true;
    }

    public abstract void Craft();

    public abstract bool CanCraft();
}