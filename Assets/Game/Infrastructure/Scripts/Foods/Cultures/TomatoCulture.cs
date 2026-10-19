using Zenject;

public class TomatoCulture : Culture
{
    public TomatoCulture(TomatoCultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
        : base(config, playerCultures, dayCycleService) { }

    public class Factory : PlaceholderFactory<TomatoCulture> { }

    public override CultureType Type => CultureType.Tomato;
}