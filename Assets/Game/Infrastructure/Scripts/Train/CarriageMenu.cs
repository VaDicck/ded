using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class CarriageMenu : MonoBehaviour, IDisposable
{
    [SerializeField] private RectTransform _quotaHolder;
    [SerializeField] private RectTransform _rewardHolder;
    
    [SerializeField] private RewardElement _rewardElementPrefab;
    
    private readonly Dictionary<CultureType, QuotaStringView> _culturesStrings = new();
    private readonly List<RewardElement> _rewardElements = new();
    
    private QuotaStringView.Factory _stringFactory;

    private QuotaElement _quotaElement;

    private GameConfig _config;
    
    [Inject]
    private void Init(QuotaStringView.Factory stringFactory, GameConfig gameConfig)
    {
        _stringFactory = stringFactory;
        _config = gameConfig;
    }

    public void Initialize(QuotaElement quota)
    {
        _quotaElement = quota;
        _quotaElement.Added += UpdateCulture;

        foreach (var expectedPair in _quotaElement.Expected)
        {
            var quotaString = _stringFactory.Create(_quotaHolder);
            quotaString.Initialize(expectedPair.Key, expectedPair.Value, _quotaElement.Actual.GetValueOrDefault(expectedPair.Key, 0));
            _culturesStrings.TryAdd(expectedPair.Key, quotaString);
        }

        foreach (var ingredient in _quotaElement.Reward.Ingredients)
        {
            var type = ingredient.Key;
            
            var rewardElement = Instantiate(_rewardElementPrefab, _rewardHolder);
            rewardElement.transform.localScale = Vector3.one;
            rewardElement.transform.localPosition = Vector3.zero;
            rewardElement.transform.localRotation = Quaternion.identity;
            rewardElement.Initialize(_config.Resources.Sprites.Find(x => x.Key == type).Value, ingredient.Value);
            _rewardElements.Add(rewardElement);
        }

        if (_quotaElement.Reward.ExtraReward != null)
        {
            var rewardElement = Instantiate(_rewardElementPrefab);
            rewardElement.transform.localScale = Vector3.one;
            rewardElement.transform.localPosition = Vector3.zero;
            rewardElement.transform.localRotation = Quaternion.identity;
            rewardElement.Initialize(_quotaElement.Reward.ExtraReward.Icon, 1);
            _rewardElements.Add(rewardElement);
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
        
        foreach (var rewardElement in _rewardElements)
            Destroy(rewardElement.gameObject);
        
        _culturesStrings.Clear();
        _rewardElements.Clear();
    }
}