using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftTableElementView : MonoBehaviour
{
    public event Action<CraftTableElementView> Clicked;
    
    public CraftProduct Product => _craftProduct;
    
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _craftAmount;
    [SerializeField] private Image _icon;
    
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _ingredientsHolder;
    
    [Header("Prefabs")]
    [SerializeField] private CraftTableElementIngredientView _ingredientPrefab;
    
    private CraftProduct _craftProduct;
    
    private readonly List<CraftTableElementIngredientView> _ingredients = new();
    
    private void Start()
    {
        _button.onClick.AddListener(() => Clicked?.Invoke(this));
    }
    
    public void Initialize(CraftProduct craftProduct)
    {
        _craftProduct = craftProduct;
        _name.text = craftProduct.Name;
        _craftAmount.text = craftProduct.CraftAmount.ToString();
        _icon.sprite = craftProduct.Sprite;

        foreach (var pair in _craftProduct.Ingredients)
        {
            var ingredient = Instantiate(_ingredientPrefab, _ingredientsHolder);
            ingredient.Initialize(pair.Key.Sprite, pair.Key.Name, pair.Value);
            _ingredients.Add(ingredient);
        }
    }

    public void Select()
    {
        
    }

    public void Deselect()
    {
        
    }

    private void OnDisable()
    {
        foreach (var ingredient in _ingredients)
        {
            Destroy(ingredient.gameObject);
        }
        
        _ingredients.Clear();
    }
}