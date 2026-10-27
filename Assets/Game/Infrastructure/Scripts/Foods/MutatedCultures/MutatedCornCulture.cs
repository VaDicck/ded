using UnityEngine;
using Zenject;

public class MutatedCornCulture: MutatedCulture
{
    public MutatedCornCulture(CultureConfig config, PlayerCultures playerCultures,
        DayCycleService dayCycleService, Transform bedTransform)
        : base(config, playerCultures, dayCycleService, bedTransform)
    {
    }

    public override CultureType Type => CultureType.Corn;
    
    [Inject] private MutatedCorn.Factory _mutatedCornFactory;
    
    protected override void OnSetTransform()
    {
        _mutatedCornFactory.Create();
    }
}