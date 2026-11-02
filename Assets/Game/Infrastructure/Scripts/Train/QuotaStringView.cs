using TMPro;
using UnityEngine;
using Zenject;

public class QuotaStringView : MonoBehaviour
{
    [field: SerializeField] private TMP_Text _text;

    public class Factory : PlaceholderFactory<QuotaStringView>
    {
        public QuotaStringView Create(Transform parent)
        {   
            var obj = Create();
            
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one * 3;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            return obj;
        }
    }

    private CulturesConfig _culturesConfig;
    
    [Inject]
    private void Initialize(GameConfig config)
    {
        _culturesConfig = config.Cultures;
    }
    
    private CultureType _cultureType;
    private int _expected;
    
    public void Initialize(CultureType type, int expected, int actual)
    {
        _expected = expected;
        _cultureType = type;
        UpdateActual(actual);
    }

    public void UpdateActual(int actual)
    {
        _text.text = $"{_culturesConfig.Names.Find(x => x.Key == _cultureType).Value}: {actual}/{_expected}";
    }
}