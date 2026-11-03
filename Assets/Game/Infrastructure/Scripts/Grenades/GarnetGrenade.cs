using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GarnetGrenade : BaseGrenade
{
    public event Action Exploded;
    
    [SerializeField] private TomatoGrenade _piecePrefab;
    [SerializeField] private float _force;
    [SerializeField] private float _explodeTime;
    [SerializeField] private List<Vector3> _directions;
    [SerializeField] private GameObject _visuals;
    [SerializeField] private VisualEffect _effect;
    
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
        _visuals.SetActive(false);
        _effect?.gameObject.SetActive(true);
        _audioSource.PlayOneShot(_audioClip);
        Destroy(gameObject, 3);
    }
}
