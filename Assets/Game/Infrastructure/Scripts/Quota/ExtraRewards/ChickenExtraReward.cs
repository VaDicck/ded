using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ChickenExtraReward", menuName = "Configs/Quotas/Rewards/Chicken")]
public class ChickenExtraReward : QuotaExtraReward
{
    private ChickenSpawner _chickenSpawner;

    [Inject]
    private void Initialize(ChickenSpawner spawner)
    {
        _chickenSpawner = spawner;
    }
    
    public override void GetReward()
    {
        _chickenSpawner.Spawn();
    }
}