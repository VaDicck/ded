using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CulturesRadialMenuElement : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private CultureType _cultureType;
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Image _cultureImage;
    
    private CulturesSelection _culturesSelection;

    private bool _isUnlocked = false;
    
    [Inject]
    private void Initialize(CulturesSelection culturesSelection)
    {
        _culturesSelection = culturesSelection;
    }
    
    private void Awake()
    {
        if (_icon == null)
            _icon = GetComponent<Image>();
    }

    public void Show()
    {
        _isUnlocked = _culturesSelection.IsUnlocked(_cultureType);
        _cultureImage.gameObject.SetActive(_isUnlocked);
    }
    
    public void Select()
    {
        _icon.sprite = _selectedSprite;
    }

    public void Deselect()
    {
        _icon.sprite = _normalSprite;
    }
    
    public void ApplyCulture()
    {
        if (_isUnlocked == false)
            return;
        
        _culturesSelection.Select(_cultureType);
    }
}