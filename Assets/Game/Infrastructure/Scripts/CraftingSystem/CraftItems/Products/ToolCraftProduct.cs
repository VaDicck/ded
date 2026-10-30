using System.Linq;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ToolCraftProduct", menuName = "CraftingSystem/Products/ToolCraftProduct")]
public class ToolCraftProduct : TypedCraftProduct<ToolType>
{
    private PlayerTools _playerTools;
    
    [Inject]
    private void Initialize(PlayerTools playerTools)
    {
        _playerTools = playerTools;
    }
    
    protected override void GiveProduct()
    {
        _playerTools.Add(Item);
    }

    public override bool CanCraft() => _playerTools.Tools.Contains(Item) == false;
}