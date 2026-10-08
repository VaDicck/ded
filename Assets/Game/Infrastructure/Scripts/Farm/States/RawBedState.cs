using System;
using Cysharp.Threading.Tasks;

public class RawBedState : IBedState
{
    public bool IsInteractable => false;
    public bool IsUsable => true;
    public event Action Performed;
    
    private ToolType _toolType;

    public RawBedState(ToolType toolType)
    {
        _toolType = toolType;
    }
    
    public async UniTask<bool> Use(ToolType toolType)
    {
        if (toolType != _toolType)
            return false;
        
        
        Performed?.Invoke();

        return true;
    }
}