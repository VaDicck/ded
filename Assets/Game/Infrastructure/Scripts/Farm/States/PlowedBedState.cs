using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlowedBedState : IBedState
{
    public bool IsInteractable => true;
    public bool IsUsable => false;
    
    public event Action Performed;
    
    public Culture Culture => _culture;
    public Type CultureType => _cultureType;
    
    private Culture _culture;
    private Type _cultureType;
    
    private GameObject _visualState;

    public PlowedBedState(GameObject visualState)
    {
        _visualState = visualState;
    }
    
    public UniTask Interact()
    {
        Performed?.Invoke();
        return UniTask.CompletedTask;
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
