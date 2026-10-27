using Zenject;

public class CarrotCulture : SimpleCulture
{
    public CarrotCulture(CarrotCultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
        : base(config, playerCultures, dayCycleService) { }
    
    public class Factory : PlaceholderFactory<CarrotCulture> {}
    
    public override CultureType Type => CultureType.Carrot;

    protected override void CreateMutatedCulture()
    {
        MutatedCarrotCulture mutated = _diContainer.Instantiate<MutatedCarrotCulture>(
            new object[] { _config, _playerCultures, _dayCycleService, _bedTransform });
        
        _plantedBedState.SetCulture(mutated);
    }
}