using UnityEngine;

public abstract class TypedCraftProduct<T> : CraftProduct
{
    [field: SerializeField] public T Item { get; private set; }
}