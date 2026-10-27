using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damage, Vector3 hitPoint)
    {
        this.LogError("PLAYER HIIIIIIIIIIT");
    }
}
