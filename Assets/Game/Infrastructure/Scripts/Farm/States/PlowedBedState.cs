using System;
using Cysharp.Threading.Tasks;

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
        
        return UniTask.CompletedTask;
    }
}
