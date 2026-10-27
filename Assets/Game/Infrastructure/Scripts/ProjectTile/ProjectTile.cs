using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifetime = 5f; 

    [SerializeField] private Rigidbody _rb;

    private void Awake()
    {
        Destroy(gameObject, _lifetime);
    }
    
    public void Launch(Vector3 direction, float speed)
    {
        _rb.linearVelocity = direction * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(0f, Vector3.zero);
            }
        }

        Destroy(gameObject);
    }
}