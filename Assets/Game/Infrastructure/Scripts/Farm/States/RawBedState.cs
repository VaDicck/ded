using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RawBedState : IBedState
{
    public bool IsInteractable => false;
    public bool IsUsable => true;
    public ToolType UseTool => _toolType;
    
    public event Action Performed;
    
    private ToolType _toolType;
    
    private GameObject _visualState;

    public RawBedState(ToolType toolType, GameObject visualState)
    {
        _toolType = toolType;
        _visualState = visualState;
    }
    
    public async UniTask<bool> Use(ToolType toolType)
    {
        if (toolType != _toolType)
            return false;
        
        
        Performed?.Invoke();

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