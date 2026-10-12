using TMPro;
using UnityEngine;
using Zenject;

public class QuotaStringView : MonoBehaviour
{
    [field: SerializeField] private TMP_Text _text;

    public class Factory : PlaceholderFactory<QuotaStringView> { }

    private QuotaStringViewConfig _config;
    
    [Inject]
    private void Init(GameConfig config)
    {
        _config = config.QuotaStringView;
    }

    public void Initialize(string text)
    {
        
    }
}