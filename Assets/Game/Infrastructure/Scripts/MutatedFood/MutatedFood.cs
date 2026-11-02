using System;
using UnityEngine;
using R3;

public class MutatedFood : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    [SerializeField] protected GameObject _vfxSad;
    
    [Header("Audio")]
    [SerializeField] protected AudioSource _audioSource;

    private Culture _culture;
    private IDisposable _subscription;

    public void SetCulture(Culture culture)
    {
        _culture = culture;

        _subscription = _culture.IsPoured.Subscribe(OnPour);
    }
    
    public void PlaceOnBed(Transform bedTransform)
    {
        transform.SetParent(bedTransform, false);
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void OnPour(bool value)
    {
        _vfxSad.SetActive(!value);
    }
    
    private void OnDestroy()
    {
        _subscription?.Dispose(); 
    }
}