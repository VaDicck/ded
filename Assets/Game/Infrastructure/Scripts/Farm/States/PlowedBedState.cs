using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlowedBedState : IBedState
{
    public bool IsInteractable => true;
    public bool IsUsable => false;
    
    public event Action Performed;
    
    public Culture Culture => _culture;
    
    private Culture _culture;
    private Type _cultureType;
    
    private readonly GameObject _visualState;
    private readonly CarrotCulture.Factory _factory; //TODO: rewrite
    
    public PlowedBedState(GameObject visualState, CarrotCulture.Factory factory)
    {
        _visualState = visualState;
        _factory = factory;
    }
    
    public UniTask Interact()
    {
        //TODO: radial menu
        
        _culture = _factory.Create();
        
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
