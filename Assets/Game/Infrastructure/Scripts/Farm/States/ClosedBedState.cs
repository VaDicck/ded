using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ClosedBedState : IBedState
{
    public event Action Performed;
    
    public bool IsInteractable => false;
    public bool IsUsable => true;

    private readonly ToolType _toolType;
    
    private GameObject _visualState;
        
    public ClosedBedState(ToolType toolType, GameObject visualState)
    {
        _toolType = toolType;
        _visualState =  visualState;
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
        ActivateVisualState(true);
    }

    public void Exit()
    {
        ActivateVisualState(false);
    }
    
    private void ActivateVisualState(bool isActive)
    {
        _visualState.gameObject.SetActive(isActive);
    }
}
