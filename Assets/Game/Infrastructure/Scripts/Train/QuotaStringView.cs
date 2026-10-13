using TMPro;
using UnityEngine;
using Zenject;

public class QuotaStringView : MonoBehaviour
{
    [field: SerializeField] private TMP_Text _text;

    public class Factory : PlaceholderFactory<QuotaStringView> { }

    private QuotaStringViewConfig _config;
    private CultureType _cultureType;
    private int _expected;
    
    [Inject]
    private void Init(GameConfig config)
    {
        _config = config.QuotaStringView;
    }

    public void Initialize(CultureType type, int expected, int actual)
    {
        _expected = expected;
        _cultureType = type;
        UpdateActual(actual);
    }

    public void UpdateActual(int actual)
    {
        _text.text = $"{_cultureType}: {actual}/{_expected}";
    }
}