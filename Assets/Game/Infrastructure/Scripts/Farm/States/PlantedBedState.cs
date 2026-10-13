using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlantedBedState : IBedState
{
    public event Action Performed;

    public bool IsInteractable => true;
    public bool IsUsable => false;

    private Type _cultureType;
    private Culture _culture;
    
    private GameObject _visualState;

    public PlantedBedState(GameObject visualState)
    {
        _visualState = visualState;
    }
    
    public void SetCulture(Type cultureType, Culture culture)
    {
        _cultureType = cultureType;
        _culture = culture;
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