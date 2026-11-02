using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class AlienBroken : MonoBehaviour
{
    public event Action Destroyed;
    
    public class Factory : PlaceholderFactory<AlienBroken> { }

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
    
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _boomSound;
    
    [Inject] private DayCycleService _dayCycleService;

    private void Start()
    {
        _dayCycleService.DayStarted += OnDayStarted;

        _audioSource.PlayOneShot(_boomSound);
        
        Vector3 explosionPos = transform.position + Vector3.up * _centerHeight;

        foreach (var rb in _rigidbodies)
        {
            rb.AddExplosionForce(_explosionForce, explosionPos, _explosionRadius, _upwardsModifier, ForceMode.Impulse);

            rb.AddTorque(Random.onUnitSphere * Random.Range(_torqueMin, _torqueMax), ForceMode.Impulse);
        }

        Destroy(gameObject, _timeLife);
    }

    private void OnDayStarted()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _dayCycleService.DayStarted -= OnDayStarted;
        Destroyed?.Invoke();
    }
}