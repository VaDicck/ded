using Zenject;

public class CarrotCulture : MutableSimpleCulture<MutatedCarrotCulture>
{
    public CarrotCulture(CarrotCultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
        : base(config, playerCultures, dayCycleService) { }
    
    public class Factory : PlaceholderFactory<CarrotCulture> {}
    
    public override CultureType Type => CultureType.Carrot;
}