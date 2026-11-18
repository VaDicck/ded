using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CurrentQuotaHUD : MonoBehaviour
{
    [SerializeField] private Transform _rowsParent;
    [SerializeField] private QuotaCultureRow _rowPrefab;

    [Inject] private QuotaService _quotaService;
    [Inject] private GameConfig _config;

    private Quota _currentQuota;
    private readonly Dictionary<CultureType, QuotaCultureRow> _activeRows = new();

    private void Start()
    {
        _quotaService.QuotaReached += RefreshCurrentQuota;
        RefreshCurrentQuota();
    }

    private void RefreshCurrentQuota()
    {
        UnsubscribeFromQuota(_currentQuota);
        _currentQuota = _quotaService.CurrentQuota;
        SubscribeToQuota(_currentQuota);
        RebuildUI();
    }

    private void SubscribeToQuota(Quota quota)
    {
        if (quota == null) return;
        quota.Main.Added += OnCultureUpdated;
        quota.Main.Reached += OnElementReached;
        quota.Sub1.Added += OnCultureUpdated;
        quota.Sub1.Reached += OnElementReached;
        quota.Sub2.Added += OnCultureUpdated;
        quota.Sub2.Reached += OnElementReached;
    }

    private void UnsubscribeFromQuota(Quota quota)
    {
        if (quota == null) return;
        quota.Main.Added -= OnCultureUpdated;
        quota.Main.Reached -= OnElementReached;
        quota.Sub1.Added -= OnCultureUpdated;
        quota.Sub1.Reached -= OnElementReached;
        quota.Sub2.Added -= OnCultureUpdated;
        quota.Sub2.Reached -= OnElementReached;
    }

    private void OnCultureUpdated(CultureType type, int _) => RebuildUI();
    private void OnElementReached() => RebuildUI();

    private void RebuildUI()
    {
        foreach (var row in _activeRows.Values)
            Destroy(row.gameObject);
        _activeRows.Clear();

        if (_currentQuota == null) return;

        var requiredAll = new Dictionary<CultureType, int>();
        var actualAll = new Dictionary<CultureType, int>();

        CollectFromElement(_currentQuota.Main, requiredAll, actualAll);
        CollectFromElement(_currentQuota.Sub1, requiredAll, actualAll);
        CollectFromElement(_currentQuota.Sub2, requiredAll, actualAll);

        foreach (var kvp in requiredAll)
        {
            var culture = kvp.Key;
            var required = kvp.Value;
            var current = actualAll.GetValueOrDefault(culture, 0);

            if (current >= required)
                continue;

            var row = Instantiate(_rowPrefab, _rowsParent);
            var icon = _config.Cultures.Sprites.Find(x => x.Key == culture)?.Value;
            row.SetData(icon, current, required);
            _activeRows[culture] = row;
        }
    }

    private void CollectFromElement(QuotaElement element,
        Dictionary<CultureType, int> required, Dictionary<CultureType, int> actual)
    {
        if (element == null || element.IsEmpty || element.IsReached)
            return;

        foreach (var pair in element.Expected)
        {
            if (required.ContainsKey(pair.Key))
                required[pair.Key] += pair.Value;
            else
                required[pair.Key] = pair.Value;

            int current = element.Actual.TryGetValue(pair.Key, out var val) ? val : 0;

            if (actual.ContainsKey(pair.Key))
                actual[pair.Key] += current;
            else
                actual[pair.Key] = current;
        }
    }

    private void OnDestroy()
    {
        _quotaService.QuotaReached -= RefreshCurrentQuota;
        UnsubscribeFromQuota(_currentQuota);
        foreach (var row in _activeRows.Values)
            Destroy(row.gameObject);
        _activeRows.Clear();
    }
}