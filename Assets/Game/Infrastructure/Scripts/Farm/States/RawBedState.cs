using System;

public class RawBedState : IBedState
{
    public event Action Performed;
    
    private ToolType _toolType;

    public RawBedState(ToolType toolType)
    {
        _toolType = toolType;
    }
    
    public bool Use(ToolType toolType)
    {
        if (toolType != _toolType)
            return false;
        
        
        
        Performed?.Invoke();

        return true;
    }
}