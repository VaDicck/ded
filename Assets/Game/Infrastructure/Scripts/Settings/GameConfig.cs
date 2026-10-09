using UnityEngine;

public class GameConfig : ScriptableObject
{
    [field: SerializeField] public float DayLength { get; private set; }
    [field: SerializeField] public int FoodCapacity { get; private set; }
}
