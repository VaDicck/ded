using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ToolsRadialMenuElement : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private ToolType _toolType;
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _normalSprite;
    
    private PlayerTools _playerTools;

    [Inject]
    private void Initialize(PlayerTools playerTools)
    {
        _playerTools = playerTools;
    }
    
    private void Awake()
    {
        if (_icon == null)
            _icon = GetComponent<Image>();
    }
    
    public void Select()
    {
        _icon.sprite = _selectedSprite;
    }

    public void Deselect()
    {
        _icon.sprite = _normalSprite;
    }
    
    public void ApplyTool()
    {
        _playerTools.Take(_toolType);
    }
}