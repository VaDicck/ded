using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftTableCraftView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _productName;
    [SerializeField] private TMP_Text _productDescription;
    [SerializeField] private Image _productImage;
    [SerializeField] private TMP_Text _productAmount;
    [Space]
    [SerializeField] private RectTransform _contentHolder;
    [SerializeField] private RectTransform _ingredientsHolder;
    [SerializeField] private Button _craftButton;
    
    [Header("Prefabs")]
    [SerializeField] private CraftTableElementIngredientView _ingredientPrefab;
    
    private readonly List<CraftTableElementIngredientView> _ingredients = new();
    
    private CraftProduct _craftProduct;

    private void Start()
    {
        _craftButton.onClick.AddListener(Craft);
        Hide();
    }
    
    public void Show(CraftProduct product)
    {
        Hide();

        _craftProduct = product;
        
        _contentHolder.gameObject.SetActive(true);
        
        _productName.text = product.Name;
        _productDescription.text = product.Description;
        _productImage.sprite = product.Sprite;
        
        if (product.CraftAmount > 1)
            _productAmount.text = product.CraftAmount.ToString();
        else
            _productAmount.text = "";

        foreach (var ingredientPair in product.Ingredients)
        {
            var ingredientView = Instantiate(_ingredientPrefab, _ingredientsHolder);
            ingredientView.Initialize(ingredientPair.Key.Sprite, ingredientPair.Key.Name, ingredientPair.Value);
            _ingredients.Add(ingredientView);
        }
        
        _craftButton.interactable = product.CanCraft() && product.IsEnoughToCraft();
    }

    public void Hide()
    {
        ClearIngredients();
        _contentHolder.gameObject.SetActive(false);
    }

    private void Craft()
    {
        if (_craftProduct == null)
            return;
        
        _craftProduct.Craft();
        
        _craftButton.interactable = _craftProduct.CanCraft() && _craftProduct.IsEnoughToCraft();
    }

    private void ClearIngredients()
    {
        foreach (var ingredient in _ingredients)
        {
            Destroy(ingredient.gameObject);
        }
        
        _ingredients.Clear();
    }

    private void OnDestroy()
    {
        _craftButton.onClick.RemoveListener(Craft);
    }
}