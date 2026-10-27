using UnityEditor;
using UnityEngine;
using Zenject;

public abstract class MutatedCulture<TFactory, TMutated> : Culture 
    where TFactory : PlaceholderFactory<TMutated>
    where TMutated : MutatedFood
{
    [Inject] private TFactory _factory;
    
    protected MutatedFood _mutatedFood;
    
    protected MutatedCulture(CultureConfig config, PlayerCultures playerCultures,
        DayCycleService dayCycleService, Transform bedTransform)
        : base(config, playerCultures, dayCycleService)
    {
        _timerDigOut = _config.HealthMutated;
        _bedTransform = bedTransform;
    }
    
    public override bool IsInteract()
    {
        return false;
    }
    
    public override bool IsUsable()
    {
        return false;
    }
    
    public override void Interact()
    {
    }
    
    public override ToolType UseTool()
    {
        return ToolType.None;
    }
    
    public override bool UseTool(ToolType toolType)
    {
        return false;
    }

    protected override void DigOut()
    {
        _mutatedFood.Die();
    }

    protected override void OnSetTransform()
    {
        _mutatedFood = _factory.Create();
        
        _mutatedFood.PlaceOnBed(_bedTransform);
    }
}
