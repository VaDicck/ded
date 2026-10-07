using System;
using Cysharp.Threading.Tasks;

public class ClosedBedState : IBedState
{
    public event Action Performed;

    private readonly ToolType _toolType;
        
    public ClosedBedState(ToolType toolType)
    {
        _toolType = toolType;
    }

    public async UniTask<bool> Use(ToolType tool)
    {
        if (tool != _toolType) 
            return false;

        var res = await Affect();

        if (res == false)
            return false;
        
        Performed?.Invoke();
        return true;
    }

    private async UniTask<bool> Affect()
    {
        // TODO: minigame
        
        return false;
    }
}
