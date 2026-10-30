using UnityEngine;

public class MutatedFood : MonoBehaviour
{
    [SerializeField] protected Animator _animator;

    public void PlaceOnBed(Transform bedTransform)
    {
        transform.SetParent(bedTransform, false);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
