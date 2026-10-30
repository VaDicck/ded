using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private RectTransform _contentFitter;
    [SerializeField] private InventoryElement _inventoryElementPrefab;
    
    [SerializeField] private Button _culturesButton;
    [SerializeField] private Button _resourcesButton;

    private readonly List<InventoryElement> _inventoryElements = new();
    
    private PlayerCultures _playerCultures;
    private PlayerResources _playerResources;

    private GameConfig _gameConfig;
    private InputController _inputController;
    
    [Inject]
    private void Initialize(PlayerCultures cultures, PlayerResources playerResources, GameConfig gameConfig, InputController inputController)
    {
        _playerCultures = cultures;
        _playerResources = playerResources;
        
        _gameConfig = gameConfig;
        _inputController = inputController;
        _inputController.OpenInventory += Show;
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(Hide);
        _culturesButton.onClick.AddListener(OpenCultures);
        _resourcesButton.onClick.AddListener(OpenResources);
    }

    private void OnEnable()
    {
        OpenCultures();
    }
    
    private void Show()
    {
        gameObject.SetActive(true);
        _inputController.BackPressed += Hide;
        _inputController.SwitchToUi();
        Time.timeScale = 0;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        _inputController.BackPressed -= Hide;
        _inputController.SwitchToGameplay();
        Time.timeScale = 1;
    }
    
    private void OpenCultures()
    {
        ClearElements();

        foreach (var pair in _playerCultures.Cultures)
        {
            var element = Instantiate(_inventoryElementPrefab, _contentFitter);
            Sprite sprite = _gameConfig.Cultures.Sprites.Find(x => x.Key == pair.Key).Value;
            string cultureName = _gameConfig.Cultures.Names.Find(x => x.Key == pair.Key).Value;
            element.Initialize(sprite, cultureName, pair.Value, _gameConfig.FoodCapacity);
            _inventoryElements.Add(element);
        }
    }

    private void OpenResources()
    {
        ClearElements();

        foreach (var pair in _playerResources.Ingredients)
        {
            var element = Instantiate(_inventoryElementPrefab, _contentFitter);
            Sprite sprite = _gameConfig.Resources.Sprites.Find(x => x.Key == pair.Key).Value;
            string resourceName = _gameConfig.Resources.Names.Find(x => x.Key == pair.Key).Value;
            element.Initialize(sprite, resourceName, pair.Value);
            _inventoryElements.Add(element);
        }
    }

    private void ClearElements()
    {
        foreach (var element in _inventoryElements)
        {
            Destroy(element.gameObject);
        }
        
        _inventoryElements.Clear();
    }
}
