using UnityEngine;

public class MutatedCornCulture : MutatedCulture<MutatedCorn.Factory, MutatedCorn>
{
    public MutatedCornCulture(CultureConfig config, PlayerCultures playerCultures,
        DayCycleService dayCycleService, Transform bedTransform)
        : base(config, playerCultures, dayCycleService, bedTransform) { }

    public override CultureType Type => CultureType.MutatedCorn;
    
    protected override void OnDayEnded()
    {
        
    }

    protected override void OnNightEnded()
    {
        this.Log("Кончилась ночь выключаем воду");
        _isPoured.Value = false;
    }
}