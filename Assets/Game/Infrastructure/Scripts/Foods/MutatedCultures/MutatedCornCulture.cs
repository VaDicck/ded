using UnityEngine;

public class MutatedCornCulture : MutatedCulture<MutatedCorn.Factory, MutatedCorn>
{
    public MutatedCornCulture(CultureConfig config, PlayerCultures playerCultures,
        DayCycleService dayCycleService, Transform bedTransform)
        : base(config, playerCultures, dayCycleService, bedTransform)
    {
    }

    public override CultureType Type => CultureType.Corn;
}