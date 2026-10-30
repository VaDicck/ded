using UnityEngine;

public class MutatedPepperCulture : MutatedCulture<MutatedPepper.Factory, MutatedPepper>
{
    public MutatedPepperCulture(CultureConfig config, PlayerCultures playerCultures,
    DayCycleService dayCycleService, Transform bedTransform)
    : base(config, playerCultures, dayCycleService, bedTransform) { }

    public override CultureType Type => CultureType.MutatedPepper;

    public override bool IsInteract()
    {
        return false;
    }

    public override void Interact()
    {

    }

    public override ToolType UseTool()
    {

        if (!_isPoured.Value)
            return ToolType.Bailer;

        return ToolType.None;
    }

    protected override void OnNightStart()
    {
        if (_isPoured.Value)
        {
            //TODO zakriza
        }
        else
        {
            //TODO rasstroiza
        }
    }

    protected override void OnNightEnded()
    {

        _isPoured.Value = false;
    }
}
