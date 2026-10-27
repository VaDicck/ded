using UnityEngine;

public class MutatedFood : MonoBehaviour
{
    public void PlaceOnBed(Transform bedTransform)
    {
        transform.SetParent(bedTransform, false);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
