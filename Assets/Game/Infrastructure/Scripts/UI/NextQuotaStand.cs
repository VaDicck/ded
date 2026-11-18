using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NextQuotaStand : MonoBehaviour
{
    [SerializeField] private TMP_Text _headerText; 
    [SerializeField] private Transform _iconsParent;
    [SerializeField] private GameObject _cultureIconPrefab; 
    
    [Inject] private QuotaService _quotaService;
    [Inject] private DayCycleService _dayCycleService;
    [Inject] private GameConfig _config;

    private bool _canUpdate;
    
    private void Start()
    {
        _quotaService.QuotaReached += OnQuotaReached;
        _dayCycleService.DayStarted += ShowNextQuota;
        _canUpdate = true;
        ShowNextQuota();
    }

    private void OnQuotaReached() => _canUpdate = true;
    
    private void ShowNextQuota()
    {
        if (!_canUpdate)
            return;
        
        _canUpdate = false;
        foreach (Transform child in _iconsParent)
        {
            Destroy(child.gameObject);
        }
        
        Quota nextQuota = _quotaService.GetNextQuotaInfo();
        if (nextQuota == null)
        {
            _headerText.text = "НЕТ СЛЕДУЮЩЕЙ КВОТЫ";
            return;
        }
        
        _headerText.text = "СЛЕДУЮЩАЯ КВОТА ПОТРЕБУЕТ";
        
        Dictionary<CultureType, int> requiredCultures = new();
        CollectRequirements(nextQuota.Main, requiredCultures);
        CollectRequirements(nextQuota.Sub1, requiredCultures);
        CollectRequirements(nextQuota.Sub2, requiredCultures);
        
        foreach (var kvp in requiredCultures)
        {
            var iconView = Instantiate(_cultureIconPrefab, _iconsParent);
            iconView.GetComponentInChildren<TMP_Text>().text = kvp.Value.ToString();
            iconView.GetComponentInChildren<Image>().sprite = GetCultureSprite(kvp.Key);
        }
    }
    
    private void CollectRequirements(QuotaElement element, Dictionary<CultureType, int> result)
    {
        if (element == null || element.IsEmpty) return;
        foreach (var pair in element.Expected)
        {
            if (result.ContainsKey(pair.Key))
                result[pair.Key] += pair.Value;
            else
                result[pair.Key] = pair.Value;
        }
    }
    
    private Sprite GetCultureSprite(CultureType type)
    {
        return _config.Cultures.Sprites.Find(x => x.Key == type).Value;
    }
    
    private void OnDestroy()
    {
        _quotaService.QuotaReached -= OnQuotaReached; 
        _dayCycleService.DayStarted -= ShowNextQuota;
    }
}