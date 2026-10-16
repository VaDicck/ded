using Zenject;

public class PotatoCulture : Culture
{
    public PotatoCulture(PotatoCultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
        : base(config, playerCultures, dayCycleService) { }

    public class Factory : PlaceholderFactory<PotatoCulture> { }

    public override CultureType Type => CultureType.Potato;
}