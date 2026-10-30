using System;
using System.Collections;
using UnityEngine;

public class TomatoGrenade : BaseGrenade
{
    public event Action Exploded;
    
    [SerializeField] private float _explosionTime;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private int _damage;
    
    protected override IEnumerator Explode()
    {
        yield return new WaitForSeconds(_explosionTime);

        var colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var col in colliders)
        {
            if (col.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_damage, col.ClosestPoint(transform.position));
            }
        }
        
        Exploded?.Invoke();
        Destroy(gameObject);
    }
}