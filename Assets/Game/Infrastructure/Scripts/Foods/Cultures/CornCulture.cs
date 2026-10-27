using Zenject;

public class CornCulture : MutableSimpleCulture<MutatedCornCulture>
{
    public CornCulture(CornCultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
        : base(config, playerCultures, dayCycleService) { }

    public class Factory : PlaceholderFactory<CornCulture> { }

    public override CultureType Type => CultureType.Corn;
}