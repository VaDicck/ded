public abstract class MutatedCulture : Culture
{
    protected MutatedCulture(CultureConfig config, PlayerCultures playerCultures, 
        DayCycleService dayCycleService) : base(config, playerCultures, dayCycleService){ }

    public override bool UseTool(ToolType toolType)
    {
        switch (toolType)
        {
            case ToolType.Bailer:
                _isPoured.Value = true;
                return true;
            
            case ToolType.Mutagen:
                OnUseMutagen();
                return true;
        }

        return false;
    }

    protected abstract void OnUseMutagen();
}
