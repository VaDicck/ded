using Zenject;

public class CornCulture : SimpleCulture
{
    public CornCulture(CornCultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
        : base(config, playerCultures, dayCycleService) { }

    public class Factory : PlaceholderFactory<CornCulture> { }

    public override CultureType Type => CultureType.Corn;
    
    protected override void CreateMutatedCulture()
    {
        MutatedCornCulture mutated = _diContainer.Instantiate<MutatedCornCulture>(
            new object[] { _config, _playerCultures, _dayCycleService, _bedTransform });
        
        _plantedBedState.SetCulture(mutated);
    }
}