using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifetime = 5f; 
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private string _layerName;
    [SerializeField] private AudioClip _audioClip;
    
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

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.isTrigger)
            return;
            
        if (collider.gameObject.layer == _layerTarget)
        {
            if (collider.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                Vector3 hitPoint = Vector3.one;
                damageable.TakeDamage(_damage, hitPoint, _audioClip);
            }
        }

        Destroy(gameObject);
    }
}