using UnityEngine;

public abstract class MutatedCulture : Culture
{
    protected MutatedCulture(CultureConfig config, PlayerCultures playerCultures,
        DayCycleService dayCycleService, Transform bedTransform)
        : base(config, playerCultures, dayCycleService)
    {
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
}
