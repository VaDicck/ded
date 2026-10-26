using Zenject;

public class CarrotCulture : MutatedCulture
{
    public CarrotCulture(CarrotCultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
        : base(config, playerCultures, dayCycleService) { }
    
    public class Factory : PlaceholderFactory<CarrotCulture> {}
    
    public override CultureType Type => CultureType.Carrot;
    
    [Inject] private MutatedCarrot.Factory _mutatedCarrotFactory;

    protected override void OnUseMutagen()
    {
        _mutatedCarrotFactory.Create();
    }
}