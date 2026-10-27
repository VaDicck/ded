public abstract class MutatedCulture : Culture
{
    protected MutatedCulture(CultureConfig config, PlayerCultures playerCultures, 
        DayCycleService dayCycleService) : base(config, playerCultures, dayCycleService){ }
    
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

    protected override void OnSetTransform()
    {
        
    }

    protected abstract void OnUseMutagen();
}
