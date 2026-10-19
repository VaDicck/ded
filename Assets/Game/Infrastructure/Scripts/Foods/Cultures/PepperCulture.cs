using Zenject;

public class PepperCulture : Culture
{
    public PepperCulture(PepperCultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
        : base(config, playerCultures, dayCycleService) { }

    public class Factory : PlaceholderFactory<PepperCulture> { }

    public override CultureType Type => CultureType.Pepper;
}