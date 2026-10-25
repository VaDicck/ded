using System;
using System.Collections.Generic;

public class PlayerResources
{
    public event Action<ResourceType> IngredientsEmpty;
    public event Action<ResourceType, int> IngredientChanged;
    
    public IReadOnlyDictionary<ResourceType, int> Ingredients => _ingredients;
    
    private readonly Dictionary<ResourceType, int> _ingredients = new();

    public void Add(ResourceType ingredient, int amount)
    {
        if (!_ingredients.TryAdd(ingredient, amount))
        {
            _ingredients[ingredient] += amount;
        }

        this.Log($"Ingredient {ingredient} has been added in the amount of {amount}");
    }

    public int Remove(ResourceType ingredient, int amount)
    {
        if (_ingredients.TryGetValue(ingredient, out var count) == false)
            return 0;

        if (count < amount)
        {
            _ingredients[ingredient] = 0;
            
            IngredientsEmpty?.Invoke(ingredient);
            
            this.Log($"taken {amount} of type: {ingredient}");
            
            return count;
        }

        _ingredients[ingredient] -= amount;
        
        IngredientChanged?.Invoke(ingredient, _ingredients[ingredient]);
        
        this.Log($"taken {amount} of type: {ingredient}");
        
        return amount;
    }

    public int CountOf(ResourceType ingredient)
    {
        return _ingredients.GetValueOrDefault(ingredient, 0);
    }
}
