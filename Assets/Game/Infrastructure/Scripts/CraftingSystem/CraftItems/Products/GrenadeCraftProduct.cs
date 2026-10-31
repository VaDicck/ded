using System.Linq;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GrenadeCraftProduct", menuName = "CraftingSystem/Products/GrenadeCraftProduct")]
public class GrenadeCraftProduct : TypedCraftProduct<ToolType>
{
    private PlayerTools _playerTools;
    private PlayerGrenades _playerGrenades;
    
    [Inject]
    private void Initialize(PlayerTools playerTools, PlayerGrenades playerGrenades)
    {
        _playerTools = playerTools;
        _playerGrenades = playerGrenades;
    }
    
    protected override void GiveProduct()
    {
        _playerTools.Add(Item);
        _playerGrenades.Add(Item);
    }

    public override bool CanCraft() => true;
}