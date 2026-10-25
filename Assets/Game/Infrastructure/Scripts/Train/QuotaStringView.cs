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
        _text.text = $"{_cultureType}: {actual}/{_expected}";
        _text.color = Color.red;
    }
}