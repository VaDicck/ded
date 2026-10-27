
public abstract class MutableSimpleCulture<TMutatedCulture> : SimpleCulture 
    where  TMutatedCulture : Culture
{
    protected MutableSimpleCulture(CultureConfig config, PlayerCultures playerCultures
        , DayCycleService dayCycleService) : base(config, playerCultures, dayCycleService) { }

    public override bool UseTool(ToolType toolType)
    {
        switch (toolType)
        {
            case ToolType.Bailer:
                _isPoured.Value = true;
                return true;
            
            case ToolType.Mutagen:
                UseMutagen();
                return true;
        }

        return false;
    }
    
    public override ToolType UseTool()
    {
        if (!_isPoured.Value)
            return ToolType.Bailer;

        return ToolType.Mutagen;
    }
    
    private void UseMutagen()
    {
        //TODO take mutagen in player
        
        CreateMutatedCulture();
    }

    private void CreateMutatedCulture()
    {
        TMutatedCulture mutated = _diContainer.Instantiate<TMutatedCulture>(
            new object[] { _config, _playerCultures, _dayCycleService, _bedTransform });

        _plantedBedState.SetCulture(mutated);
    }
}
