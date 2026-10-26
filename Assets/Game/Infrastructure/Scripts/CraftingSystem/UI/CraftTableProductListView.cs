using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CraftTableProductListView : MonoBehaviour
{
    public event Action<CraftProduct> Selected;
    
    [SerializeField] private RectTransform _elementsHolder;
    [SerializeField] private CraftTableElementView _elementPrefab;

    private readonly List<CraftTableElementView> _elements = new();
    private CraftTableElementView _selectedElement;
    
    private CraftingTableConfig _config;
    
    [Inject]
    private void Initialize(GameConfig config)
    {
        _config = config.CraftingTable;
    }
    
    public void Show()
    {
        foreach (var product in _config.Products)
        {
            var element = Instantiate(_elementPrefab, _elementsHolder);
            element.Initialize(product);
            element.Clicked += OnElementClicked;
            _elements.Add(element);
        }
    }

    private void OnElementClicked(CraftTableElementView element)
    {
        _selectedElement?.Deselect();
        _selectedElement = element;
        _selectedElement.Select();
        Selected?.Invoke(_selectedElement.Product);
    }

    public void Hide()
    {
        foreach (var element in _elements)
        {
            element.Clicked -= OnElementClicked;
            Destroy(element.gameObject);
        }
        _elements.Clear();
    }
}