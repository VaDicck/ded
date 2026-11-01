using System.Linq;
using UnityEngine;
using Zenject;

public class UiNotifications : MonoBehaviour
{
    [SerializeField] private PlusNotification _plusNotificationPrefab;
    [SerializeField] private RectTransform _holder;
    
    private PlayerCultures _cultures;
    private PlayerResources _resources;
    private PlayerMutagens _mutagens;
    private PlayerGrenades _grenades;

    private GameConfig _config;
    
    [Inject]
    private void Initialize(PlayerCultures cultures, PlayerResources resources,
        PlayerMutagens mutagens, PlayerGrenades grenades, GameConfig config)
    {
        _cultures = cultures;
        _resources = resources;
        _mutagens = mutagens;
        _grenades = grenades;
        
        _config = config;
    }

    private void Start()
    {
        _cultures.CultureAdded += OnCulture;
        _resources.ResourceAdded += OnResource;
        _mutagens.Added += OnMutagen;
        _grenades.Added += OnGrenade;
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
    
    private void CreatePlusNotification(Sprite sprite, int count)
    {
        var notification = Instantiate(_plusNotificationPrefab, _holder);
        notification.Initialize(sprite, count);
    }
}