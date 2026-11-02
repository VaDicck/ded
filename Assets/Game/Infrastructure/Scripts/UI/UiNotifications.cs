using UnityEngine;
using Zenject;

public class UiNotifications : MonoBehaviour
{
    [SerializeField] private PlusNotification _plusNotificationPrefab;
    [SerializeField] private RectTransform _holder;
    [SerializeField] private CultureOpenedNotification _cultureOpenedNotification; 
    
    private PlayerCultures _cultures;
    private PlayerResources _resources;
    private PlayerMutagens _mutagens;
    private PlayerGrenades _grenades;
    private CulturesSelection _culturesSelection;

    private GameConfig _config;
    
    [Inject]
    private void Initialize(PlayerCultures cultures, PlayerResources resources,
        PlayerMutagens mutagens, PlayerGrenades grenades, CulturesSelection culturesSelection, GameConfig config)
    {
        _cultures = cultures;
        _resources = resources;
        _mutagens = mutagens;
        _grenades = grenades;

        _culturesSelection = culturesSelection;
        
        _config = config;
    }

    private void Start()
    {
        _cultures.CultureAdded += OnCulture;
        _resources.ResourceAdded += OnResource;
        _mutagens.Added += OnMutagen;
        _grenades.Added += OnGrenade;
        _culturesSelection.CultureOpened += OnCultureOpened;
    }

    private void OnCulture(CultureType type, int count)
    {
        Sprite sprite = _config.Cultures.Sprites.Find(x => x.Key == type).Value;
        CreatePlusNotification(sprite, count);
    }

    private void OnResource(ResourceType type, int count)
    {
        Sprite sprite = _config.Resources.Sprites.Find(x => x.Key == type).Value;
        CreatePlusNotification(sprite, count);
    }

    private void OnMutagen()
    {
        Sprite sprite = _config.ToolSprites.Find(x=> x.Key == ToolType.Mutagen).Value;
        CreatePlusNotification(sprite, 1);
    }

    private void OnGrenade(ToolType type)
    {
        Sprite sprite = _config.ToolSprites.Find(x => x.Key == type).Value;
        CreatePlusNotification(sprite, 1);
    }

    private void OnCultureOpened(CultureType type)
    {
        Sprite sprite = _config.Cultures.Sprites.Find(x => x.Key == type).Value;
        string cultureName = _config.Cultures.Names.Find(x => x.Key == type).Value;
        
        _cultureOpenedNotification.Show(sprite, cultureName);
    }
    
    private void CreatePlusNotification(Sprite sprite, int count)
    {
        var notification = Instantiate(_plusNotificationPrefab, _holder);
        notification.Initialize(sprite, count);
    }
}