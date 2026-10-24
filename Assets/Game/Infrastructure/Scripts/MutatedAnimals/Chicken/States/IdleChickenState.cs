using System;

public class IdleChickenState : IChickenState
{
    public event Action Performed;

    public void Tick()
    {
        Performed?.Invoke();
    }
}
