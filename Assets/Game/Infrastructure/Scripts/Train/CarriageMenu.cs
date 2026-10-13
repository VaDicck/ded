using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CarriageMenu : MonoBehaviour, IDisposable
{
    private readonly Dictionary<CultureType, QuotaStringView> _culturesStrings = new();

    private QuotaStringView.Factory _stringFactory;

    private QuotaElement _quotaElement;
    
    [Inject]
    private void Init(QuotaStringView.Factory stringFactory)
    {
        _stringFactory = stringFactory;
    }

    public void Initialize(QuotaElement quota)
    {
        _quotaElement = quota;
        _quotaElement.Added += UpdateCulture;

        foreach (var expectedPair in _quotaElement.Expected)
        {
            var quotaString = _stringFactory.Create(transform);
            quotaString.Initialize(expectedPair.Key, expectedPair.Value, _quotaElement.Actual.GetValueOrDefault(expectedPair.Key, 0));
            _culturesStrings.Add(expectedPair.Key, quotaString);
        }
    }

    private void UpdateCulture(CultureType culture, int value)
    {
        if(_culturesStrings.TryGetValue(culture, out var cultureString))
        {
            cultureString.UpdateActual(value);
        }
    }
    
    public void Dispose()
    {
        if (_quotaElement != null)
            _quotaElement.Added -= UpdateCulture;
        _quotaElement = null;
        
        foreach (var cultureString in _culturesStrings)
            Destroy(cultureString.Value.gameObject);
        
        _culturesStrings.Clear();
    }
}