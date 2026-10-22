using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienBroken : MonoBehaviour
{
    public event Action Destroyed;

    [SerializeField] private float _explosionForce = 950f;   
    [SerializeField] private float _explosionRadius = 3.2f;    
    [SerializeField] private float _upwardsModifier = 0.8f;   
    [SerializeField] private float _centerHeight = 1.1f;      

    [SerializeField] private float _outwardForceMin = 280f;
    [SerializeField] private float _outwardForceMax = 520f;

    [SerializeField] private float _torqueMin = 90f;
    [SerializeField] private float _torqueMax = 280f;

    [SerializeField] private float _timeLife = 7f;

    [SerializeField] private Rigidbody[] _rigidbodies;   

    private void Start()
    {
        Vector3 explosionPos = transform.position + Vector3.up * _centerHeight;

        foreach (var rb in _rigidbodies)
        {
            rb.AddExplosionForce(_explosionForce, explosionPos, _explosionRadius, _upwardsModifier, ForceMode.Impulse);

            Vector3 fromCenter = rb.transform.position - explosionPos;
            if (fromCenter.sqrMagnitude > 0.001f)
            {
                rb.AddForce(fromCenter.normalized * Random.Range(_outwardForceMin, _outwardForceMax), ForceMode.Impulse);
            }

            rb.AddTorque(Random.onUnitSphere * Random.Range(_torqueMin, _torqueMax), ForceMode.Impulse);
        }

        Destroy(gameObject, _timeLife);
    }

    private void OnDestroy()
    {
        Destroyed?.Invoke();
    }
}