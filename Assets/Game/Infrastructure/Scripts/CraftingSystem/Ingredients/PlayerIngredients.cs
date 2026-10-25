using System;
using System.Collections.Generic;

public class PlayerIngredients
{
    public event Action<IngredientType> IngredientsEmpty;
    public event Action<IngredientType, int> IngredientChanged;
    
    public IReadOnlyDictionary<IngredientType, int> Ingredients => _ingredients;
    
    private readonly Dictionary<IngredientType, int> _ingredients = new();

    public void Add(IngredientType ingredient, int amount)
    {
        if (!_ingredients.TryAdd(ingredient, amount))
        {
            _ingredients[ingredient] += amount;
        }

        this.Log($"Ingredient {ingredient} has been added in the amount of {amount}");
    }

    public int Remove(IngredientType ingredient, int amount)
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

    public int CountOf(IngredientType ingredient)
    {
        return _ingredients.GetValueOrDefault(ingredient, 0);
    }
}
