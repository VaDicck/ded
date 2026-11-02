using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarnetGrenade : BaseGrenade
{
    public event Action Exploded;
    
    [SerializeField] private TomatoGrenade _piecePrefab;
    [SerializeField] private float _force;
    [SerializeField] private float _explodeTime;
    [SerializeField] private List<Vector3> _directions;
    
    protected override IEnumerator Explode()
    {
        yield return new WaitForSeconds(_explodeTime);

        foreach (var direction in _directions)
        {
            var grenade = Instantiate(_piecePrefab, transform.position, Quaternion.identity);
            grenade.Throw(direction * _force);
            this.Log($"{direction} {grenade.name}");
        }

        Exploded?.Invoke();
        Destroy(gameObject);
    }
}
