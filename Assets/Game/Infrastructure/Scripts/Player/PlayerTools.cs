using System;
using System.Collections.Generic;

public class PlayerTools
{
    public event Action<ToolType?> ToolChanged;
    public IReadOnlyCollection<ToolType> Tools => _tools;
    public ToolType? Tool => _currentTool;
    
    private ToolType? _currentTool;
    private readonly List<ToolType> _tools = new();

    private PlayerMutagens _playerMutagens;
    
    public PlayerTools(PlayerMutagens playerMutagens)
    {
        _playerMutagens = playerMutagens;

        _playerMutagens.Used += count =>
        {
            if (count == 0)
                PutOff();
        };
    }
    
    public bool Take(ToolType toolType)
    {
        if (_tools.Contains((ToolType) toolType) == false)
            return false;
        
        _currentTool = toolType;
        ToolChanged?.Invoke(_currentTool);
        this.Log($"Tool changed to: {_currentTool}");
        return true;
    }

    public bool Contains(ToolType toolType)
    {
        return _tools.Contains(toolType);
    }

    public void PutOff()
    {
        _currentTool = null;
        this.Log($"Tool put off");
        ToolChanged?.Invoke(_currentTool);
    }

    public bool Add(ToolType toolType)
    {
        if (_tools.Contains(toolType))
            return false;
        
        _tools.Add(toolType);

        return true;
    }
}