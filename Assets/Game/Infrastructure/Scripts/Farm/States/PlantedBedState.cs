using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlantedBedState : IBedState
{
    public event Action Performed;

    public bool IsInteractable => true;
    public bool IsUsable => false;

    
    // TODO: culture
    public void SetCulture(int culture)
    {
        
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