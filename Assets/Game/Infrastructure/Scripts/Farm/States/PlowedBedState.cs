using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlowedBedState : IBedState
{
    public bool IsInteractable => true;
    public bool IsUsable => false;
    
    public event Action Performed;
    
    //TODO: set culture
    public int Culture => 0;

    public UniTask Interact()
    {
        // TODO: radial menu
        Performed?.Invoke();
        return UniTask.CompletedTask;
    }
    
    public void Enter()
    {
        Debug.Log("Plowed enter");
    }
}
