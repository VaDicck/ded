using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    [SerializeField] private TMP_Text _toolName;
    [SerializeField] private TMP_Text _toolCounts;
    [SerializeField] private TMP_Text _count;
    
    private PlayerTools _playerTools;
    private PlayerMutagens _playerMutagens;
    private PlayerGrenades _playerGrenades;
    
    private List<SerializedPair<ToolType, Sprite>> _toolSprites;
    private List<SerializedPair<ToolType, string>> _toolNames;
    
    private int _currentTool;
    private float _lastSwitchTime;

    [Inject]
    private void Initialize(PlayerTools playerTools, GameConfig config, PlayerMutagens playerMutagens, PlayerGrenades playerGrenades)
    {
        _playerTools = playerTools;
        _playerMutagens = playerMutagens;
        _playerGrenades = playerGrenades;
        
        _toolSprites = config.ToolSprites;
        _toolNames = config.ToolNames;
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
        
        _count?.gameObject.SetActive(true);
        
        if (_playerTools.Contains(_tools[_currentTool]) == false)
        {
            _toolImage.gameObject.SetActive(false);
            _count?.gameObject.SetActive(false);
            
            return;
        }
        
        _toolImage.gameObject.SetActive(_playerTools.Contains(_tools[_currentTool]));
        _toolImage.sprite = _toolSprites.Find(x => x.Key == _tools[_currentTool]).Value;
        
        switch (_tools[_currentTool])
        {
            case ToolType.Mutagen:
                _count.text = _playerMutagens.Count.ToString();
                
                break;
            
            case ToolType.TomatoGrenade:
                _count.text = _playerGrenades.Count(_tools[_currentTool]).ToString();

                break;
            
            default:
                if (_count != null)
                    _count.text = "";

                break;
        }
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

    public bool SetToolText()
    {
        if (_tools.Count == 0)
            return false;

        if (_playerTools.Contains(_tools[_currentTool]) == false)
            return false;
        
        _toolName.text = _toolNames.Find(x => x.Key == _tools[_currentTool]).Value;
        int count = 0;
        int current = 0;

        foreach (var tool in _tools)
        {
            if (_playerTools.Contains(tool))
                count++;

            if (tool == _tools[_currentTool])
            {
                current =  count;
            }
        }

        if (count <= 1)
            _toolCounts.text = "";
        else
            _toolCounts.text =$"{current}/{count}";

        return true;
    }
    
    public void ApplyTool()
    {
        if (_tools.Count == 0)
            return;

        if (_tools[_currentTool] == ToolType.Mutagen && _playerMutagens.Count == 0)
            return;

        if (_tools[_currentTool] == ToolType.TomatoGrenade && _playerGrenades.Count(ToolType.TomatoGrenade) == 0)
            return;
        
        if (_playerTools.Tool == _tools[_currentTool])
            _playerTools.PutOff();
        else
        {
            _playerTools.Take(_tools[_currentTool]);
        }
    }
}