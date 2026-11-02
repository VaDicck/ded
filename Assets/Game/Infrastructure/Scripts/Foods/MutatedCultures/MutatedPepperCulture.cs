using UnityEngine;

public class MutatedPepperCulture : MutatedCulture<MutatedPepper.Factory, MutatedPepper>
{
    public MutatedPepperCulture(CultureConfig config, PlayerCultures playerCultures,
    DayCycleService dayCycleService, Transform bedTransform, MutatedPepperConfig mutatedConfig)
    : base(config, playerCultures, dayCycleService, bedTransform, mutatedConfig) { }

    public override CultureType Type => CultureType.MutatedPepper;

    public override bool IsInteract()
    {
        return false;
    }

    public override void Interact()
    {

    }

    protected override void OnNightStart()
    {
        if (_isPoured.Value)
        {
            _mutatedFood.StartAttack();
        }
    }

    protected override void OnNightEnded()
    {
        _mutatedFood.Idle();
        _isPoured.Value = false;
    }
}
