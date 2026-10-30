using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CultureCraftIngredient", menuName = "CraftingSystem/Ingredient/Culture")]
public class CultureCraftIngredient : TypedCraftIngredient<CultureType>
{
    private PlayerCultures _playerCultures;
    
    [Inject]
    private void Initialize(PlayerCultures playerCultures)
    {
        _playerCultures = playerCultures;
    }
    
    public override bool IsEnough(int amount)
    {
        return _playerCultures.Count(Item) >= amount;
    }

    public override void Craft(int amount)
    {
        _playerCultures.Take(Item, amount);
    }
}