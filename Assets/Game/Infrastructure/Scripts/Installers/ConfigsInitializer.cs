using Zenject;

public class ConfigsInitializer : IInitializable
{
    [Inject] private GameConfig _gameConfig;
    [Inject] private DiContainer _diContainer;
    
    public void Initialize()
    {
        foreach (var product in _gameConfig.CraftingTable.Products)
        {
            _diContainer.Inject(product);

            foreach (var ingredient in product.Ingredients)
            {
                _diContainer.Inject(ingredient.Key);
            }
        }
    }
}