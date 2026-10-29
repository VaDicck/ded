using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ToolsRadialMenuElement : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Image _toolImage;
    [SerializeField] private float _secondsToSwitch;
    
    [SerializeField] private List<ToolType> _tools;
    
    private PlayerTools _playerTools;
    private List<SerializedPair<ToolType, Sprite>> _toolSprites;
    
    private int _currentTool;
    private float _lastSwitchTime;

    [Inject]
    private void Initialize(PlayerTools playerTools, GameConfig config)
    {
        _playerTools = playerTools;
        _toolSprites = config.ToolSprites;
    }
    
    private void Awake()
    {
        if (_icon == null)
            _icon = GetComponent<Image>();
    }
    
    public void Show()
    {
        if (_tools.Count == 0)
        {
            _toolImage.gameObject.SetActive(false);

            return;
        }
        _toolImage.gameObject.SetActive(_playerTools.Contains(_tools[_currentTool]));
        _toolImage.sprite = _toolSprites.Find(x => x.Key == _tools[_currentTool]).Value;
    }

    public void SwitchNext()
    {
        if (Time.realtimeSinceStartup - _lastSwitchTime < _secondsToSwitch)
            return;
        
        _lastSwitchTime = Time.realtimeSinceStartup;
        
        if (_tools.Count == 0)
            return;
        
        var tool = (_currentTool + 1) % _tools.Count;

        int extra = 0;
        
        while (_playerTools.Contains(_tools[tool]) == false)
        {
            tool = (_currentTool + 1) % _tools.Count;
            extra++;

            if (extra > _tools.Count)
            {
                return;
            }
        }

        _currentTool = tool;
        _toolImage.sprite = _toolSprites.Find(x => x.Key == _tools[tool]).Value;
        Show();
    }

    public void SwitchPrevious()
    {
        if (Time.realtimeSinceStartup - _lastSwitchTime < _secondsToSwitch)
            return;
        
        _lastSwitchTime = Time.realtimeSinceStartup;
        
        if (_tools.Count == 0)
            return;
        
        int tool = _currentTool;
        
        if (tool <= 0)
            tool = _tools.Count;
        
        tool = (tool - 1) % _tools.Count;

        int extra = 0;
        
        while (_playerTools.Contains(_tools[tool]) == false)
        {
            if (tool <= 0)
                tool = _tools.Count;
            
            tool = (tool - 1) % _tools.Count;
            extra++;
            
            if (extra > _tools.Count)
            {
                return;
            }
        }

        _currentTool = tool;
        _toolImage.sprite = _toolSprites.Find(x => x.Key == _tools[tool]).Value;
        Show();
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
        if (_tools.Count == 0)
            return;
        
        if (_playerTools.Tool == _tools[_currentTool])
            _playerTools.PutOff();
        else
        {
            _playerTools.Take(_tools[_currentTool]);
        }
    }
}