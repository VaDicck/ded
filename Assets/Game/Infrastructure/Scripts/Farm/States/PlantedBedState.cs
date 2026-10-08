using System;
using Cysharp.Threading.Tasks;

public class PlantedBedState : IBedState
{
    public bool IsInteractable { get; private set; }
    public bool IsUsable => false;

    public event Action Performed;
    
    // TODO: culture
    public void SetCulture(int culture)
    {
        
    }

    public UniTask Interact()
    {
        return UniTask.CompletedTask;
    }
}