using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    [field: SerializeField] public float DayLength { get; private set; }
    [field: SerializeField] public int FoodCapacity { get; private set; }
    [field: SerializeField] public int QuotaExpireDays { get; private set; }
    [field: SerializeField] public QuotasConfig Quotas { get; private set; }
    [field: SerializeField] public QuotaStringViewConfig QuotaStringView { get; private set; }
}
