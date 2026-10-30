using UnityEngine;

public class MutatedCarrotCulture : MutatedCulture<MutatedCarrot.Factory, MutatedCarrot>
{
    public MutatedCarrotCulture(CultureConfig config, PlayerCultures playerCultures,
        DayCycleService dayCycleService, Transform bedTransform, MutatedCarrotConfig mutatedConfig)
        : base(config, playerCultures, dayCycleService, bedTransform, mutatedConfig) { }

    public override CultureType Type => CultureType.MutatedCarrot;
    protected override void OnNightStart()
    {
    }

    protected override void OnNightEnded()
    {
    }
}