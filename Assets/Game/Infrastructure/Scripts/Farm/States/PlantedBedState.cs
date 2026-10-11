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
        Debug.Log("Planted enter");
    }
}