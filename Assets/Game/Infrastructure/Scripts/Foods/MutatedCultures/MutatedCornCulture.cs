using Zenject;

public class MutatedCornCulture: MutatedCulture
{
    public MutatedCornCulture(CornCultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
        : base(config, playerCultures, dayCycleService) { }

    public override CultureType Type => CultureType.Corn;
    
    [Inject] private MutatedCorn.Factory _mutatedCornFactory;
    
    protected override void OnUseMutagen()
    {
        _mutatedCornFactory.Create();
    }
}