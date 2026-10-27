using Zenject;

public class MutatedCarrotCulture : MutatedCulture
{
    public MutatedCarrotCulture(CarrotCultureConfig config, PlayerCultures playerCultures, DayCycleService dayCycleService)
        : base(config, playerCultures, dayCycleService) { }
    
    public override CultureType Type => CultureType.Carrot;
    
    [Inject] private MutatedCarrot.Factory _mutatedCarrotFactory;

    protected override void OnUseMutagen()
    {
        _mutatedCarrotFactory.Create();
    }
}