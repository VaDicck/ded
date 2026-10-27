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
        
        _dayCycleService.DayEnded += OnDayEnded;
        _dayCycleService.NightEnded += OnNightEnded;
    }
    
    public override bool IsInteract()
    {
        return false;
    }
    
    public override bool IsUsable()
    {
        return !IsPoured.CurrentValue;
    }

    public override void Interact() { }

    public override ToolType UseTool()
    {
        if (!_isPoured.Value)
            return ToolType.Bailer;
        
        return ToolType.None;
    }
    
    public override bool UseTool(ToolType toolType)
    {
        switch (toolType)
        {
            case ToolType.Bailer:
                _isPoured.Value = true;
                return true;
        }

        return false;
    }

    protected abstract void OnDayEnded();
    protected abstract void OnNightEnded();

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
