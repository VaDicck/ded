using System;

public class PlowedBedState : IBedState
{
    public event Action Performed;

    public void Interact()
    {
        // TODO: radial menu
    }
}
