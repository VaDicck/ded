using UnityEngine;

public abstract class TypedCraftIngredient<T> : CraftIngredient
{
    [field: SerializeField] public T Item { get; private set; }
}