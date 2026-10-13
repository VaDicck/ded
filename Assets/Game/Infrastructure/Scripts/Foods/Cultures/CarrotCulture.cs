using Zenject;

public class CarrotCulture : Culture
{
    public CarrotCulture(CultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService) 
        : base(config,  playerCultures, dayCycleService) { }
    
    public class Factory : PlaceholderFactory<CarrotCulture> {}
    
    public override CultureType Type => CultureType.Carrot;
}