using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ClosedBedState : IBedState
{
    public event Action Performed;
    
    public bool IsInteractable => false;
    public bool IsUsable => true;

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
        
        return true;
    }

    public void Enter()
    {
        Debug.Log("Closed enter");
    }
}
