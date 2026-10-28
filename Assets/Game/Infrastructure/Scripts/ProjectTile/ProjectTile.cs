using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifetime = 5f; 
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private string _layerName;
    
    private int _layerTarget;
    private float _damage;

    private void Awake()
    {
        _layerTarget = LayerMask.NameToLayer(_layerName);
        
        Destroy(gameObject, _lifetime);
    }
    
    public void Launch(Vector3 direction, float speed, float damage)
    {
        _rb.linearVelocity = direction * speed;
        _damage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _layerTarget)
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                Vector3 hitPoint = collision.contacts[0].point;
                damageable.TakeDamage(_damage, hitPoint);
            }
        }

        Destroy(gameObject);
    }
}