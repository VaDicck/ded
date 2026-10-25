using System.Linq;
using UnityEngine;
using Zenject;

public class CraftTest : MonoBehaviour
{
    private CraftingTableConfig _craftingTableConfig;
    
    [Inject]
    private void Initialize(GameConfig config)
    {
        _craftingTableConfig = config.CraftingTable;
    }

    [ContextMenu("Craft")]
    private void Craft()
    {
        print(_craftingTableConfig.Products.First().CanCraft());
        _craftingTableConfig.Products.First().Craft();
    }
}
