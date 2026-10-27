using UnityEngine;
using Zenject;

public class FlyingAlien : Alien<IFlyingAlienState>
{
    public override AlienType Type => AlienType.Flying;

    public class Factory : PlaceholderFactory<FlyingAlien> { }
    
    protected override void Die()
    {
        Destroy(gameObject);
    }

    protected override void Subscribe()
    {
        
    }

    protected override void AddStates()
    {
        
    }
}