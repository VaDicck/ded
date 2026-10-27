using Zenject;

public class GarnetCulture : SimpleCulture
{
    public GarnetCulture(GarnetCultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
        : base(config, playerCultures, dayCycleService) { }

    public class Factory : PlaceholderFactory<GarnetCulture> { }

    public override CultureType Type => CultureType.Garnet;
}