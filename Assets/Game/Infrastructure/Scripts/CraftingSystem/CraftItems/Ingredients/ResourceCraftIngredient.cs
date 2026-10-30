using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ResourceCraftIngredient", menuName = "CraftingSystem/Ingredient/Resource")]
public class ResourceCraftIngredient : TypedCraftIngredient<ResourceType>
{
    private PlayerResources _playerResources;
    
    [Inject]
    public void Initialize(PlayerResources playerResources)
    {
        _playerResources = playerResources;
    }
    
    public override bool IsEnough(int amount)
    {
        return _playerResources.CountOf(Item) >= amount;
    }

    public override void Craft(int amount)
    {
        _playerResources.Remove(Item, amount);
    }
}