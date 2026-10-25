using UnityEngine;

[CreateAssetMenu(fileName = "CultureCraftIngredient", menuName = "CraftingSystem/Ingredient/Culture")]
public class CultureCraftIngredient : TypedCraftIngredient<CultureType>
{
    private PlayerCultures _playerCultures;
    
    public void Initialize(PlayerCultures playerCultures)
    {
        _playerCultures = playerCultures;
    }
    
    public override bool IsEnough(int amount)
    {
        return _playerCultures.Count(Item) >= amount;
    }
}