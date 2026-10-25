using UnityEngine;

public abstract class CraftIngredient : ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public string Name { get; private set; }

    public abstract bool IsEnough(int amount);
}
