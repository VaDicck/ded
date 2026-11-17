using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public event Action Hit;
    
    public bool IsZooming => _isZooming;
    public int MaxCountPatron => _maxCountPatron; 
    public ReadOnlyReactiveProperty<int> CurrentCountPatron => _currentCountPatron;
    
    [SerializeField] protected int _maxCountPatron = 6;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Animator _animatorHand;
    protected ReactiveProperty<int> _currentCountPatron = new(0);
    private float _lastShotTime;
    
    [Header("Audio")]
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField, Range(0, 1)] protected float _shootVolume;
    [SerializeField, Range(0, 1)] protected float _zoomVolume;
    [SerializeField] protected AudioClip _noAmmoSound;
    [SerializeField] protected AudioClip _shotSound;
    [SerializeField] protected AudioClip _zoomSound;
    [SerializeField] protected AudioClip _unzoomSound;
    
    protected Coroutine _reloadCoroutine;
    protected bool _isReloading = false;
    
    protected bool _isZooming = false;

    private void Start()
    {
        _currentCountPatron.Value = _maxCountPatron;
        _lastShotTime = Time.time;
    }
    
    public bool Shoot()
    {
        if (_currentCountPatron.CurrentValue < 1)
        {
            _audioSource.PlayOneShot(_noAmmoSound);
            Reload();
            return false;
        }

        if (!(Time.time - _lastShotTime >= _fireRate)) 
            return false;
        
        if (_isReloading)
        {
            CancelReload();
        }
        
        _animator.SetTrigger("Shot");
        _animatorHand.SetTrigger("Shot");
        
        _audioSource.PlayOneShot(_shotSound, _shootVolume);
        ReleasePatron();

        _currentCountPatron.Value--;
            
        _lastShotTime = Time.time;
        
        return true;
    }
    
    public void Reload()
    {
        if(_reloadCoroutine == null)
        {
            _isReloading = true;
            _animator.SetTrigger("StartReload");
            _animatorHand.SetTrigger("StartReload");
            _reloadCoroutine = StartCoroutine(ReloadRoutine());
        }
    }
    
    public void CancelReload()
    {
        if (_reloadCoroutine != null)
        {
            StopCoroutine(_reloadCoroutine);
            _reloadCoroutine = null;
        }
        
        _isReloading = false;
    }
    
    public void Equip()
    {
        Show();
    }

    public void Unequip()
    {
        CancelReload();
        Hide();
    }

    private void ReleasePatron()
    {
        List<RaycastHit> hits = RaycastTarget();
        
        if(hits.Count < 1)
            return;
        
        Hit?.Invoke();
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent<IDamageable>(out var damageableObject))
            {
                damageableObject.TakeDamage(damageCalculation(hit.distance), hit.point);
            }
        }
    }

    public abstract void Zoom();
    public abstract void UnZoom();

    public abstract void SetSprintSpread(bool isSprinting);

    protected abstract void Hide();
    
    protected abstract void Show();
    
    protected abstract List<RaycastHit> RaycastTarget();

    protected abstract float damageCalculation(float distance);

    protected abstract IEnumerator ReloadRoutine();
}
