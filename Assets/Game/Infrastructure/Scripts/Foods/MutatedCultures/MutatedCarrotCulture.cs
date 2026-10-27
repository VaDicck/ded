using UnityEngine;
using Zenject;

public class MutatedCarrotCulture : MutatedCulture
{
    public MutatedCarrotCulture(CultureConfig config, PlayerCultures playerCultures,
        DayCycleService dayCycleService, Transform bedTransform)
        : base(config, playerCultures, dayCycleService, bedTransform) { }

    public override CultureType Type => CultureType.Carrot;
    
    [Inject] private MutatedCarrot.Factory _mutatedCarrotFactory;
    
    protected override void OnSetTransform()
    {
        _mutatedCarrotFactory.Create();
    }
}