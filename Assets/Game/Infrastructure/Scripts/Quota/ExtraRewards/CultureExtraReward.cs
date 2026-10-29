
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CultureExtraReward", menuName = "Configs/Quotas/Rewards/Culture")]
public class CultureExtraReward : QuotaExtraReward
{
    [SerializeField] private CultureType _culture;
    
    private CulturesSelection _culturesSelection;
    
    [Inject]
    private void Initialize(CulturesSelection culturesSelection)
    {
        _culturesSelection = culturesSelection;
    }
    
    public override void GetReward()
    {
        _culturesSelection.UnlockCulture(_culture);
    }
}
