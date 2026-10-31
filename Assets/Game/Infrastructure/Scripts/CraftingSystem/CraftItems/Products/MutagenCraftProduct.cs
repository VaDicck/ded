using System.Linq;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "MutagenCraftProduct", menuName = "CraftingSystem/Products/MutagenCraftProduct")]
public class MutagenCraftProduct : TypedCraftProduct<ToolType>
{
    private PlayerTools _playerTools;
    private PlayerMutagens _playerMutagens;
    
    [Inject]
    private void Initialize(PlayerTools playerTools, PlayerMutagens playerMutagens)
    {
        _playerTools = playerTools;
        _playerMutagens = playerMutagens;
    }
    
    protected override void GiveProduct()
    {
        _playerTools.Add(Item);
        _playerMutagens.Add();
    }

    public override bool CanCraft() => true;
}