using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CulturesHolderView : MonoBehaviour
{
    private CountCultureStringView.Factory _stringFactory;
    private PlayerCultures _playerCultures;
    
    private Dictionary<CultureType, CountCultureStringView> _cultures = new();

    [Inject]
    private void Init(CountCultureStringView.Factory stringFactory, PlayerCultures playerCultures)
    {
        _stringFactory = stringFactory;

        _playerCultures = playerCultures;
    }

    private void Awake()
    {
        _playerCultures.CultureEmpty += DeleteCulture;
        _playerCultures.CultureCountChanged += ChangedCultureValue;
    }

    private void ChangedCultureValue(CultureType culture, int value)
    {
        if (_cultures.ContainsKey(culture))
        {
            _cultures[culture].UpdateActual(value);
            
            return;
        }
        
        var newCultureStringView = _stringFactory.Create(transform);
        
        _cultures.TryAdd(culture, newCultureStringView);
            
        newCultureStringView.Initialize(culture, value);
    }

    private void DeleteCulture(CultureType culture)
    {
        if (_cultures.TryGetValue(culture, out var a))
        {
            Destroy(a.gameObject);
        }
        
        _cultures.Remove(culture);
    }

    private void OnDestroy()
    {
        _playerCultures.CultureEmpty -= DeleteCulture;
        _playerCultures.CultureCountChanged -= ChangedCultureValue;
    }
}
