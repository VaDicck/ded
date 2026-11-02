using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Shotgun : Weapon
{
    [Header("Shotgun Settings")]
    [SerializeField] private float _pelletDamage = 12f;
    [SerializeField] private int _pelletCount = 8;
    [SerializeField] private float _spreadAngle = 10f;
    [SerializeField] private float _effectiveRange = 15f;
    [SerializeField] private GameObject _shotgun;
    
    [Header("Spread Modifiers")]
    [SerializeField] private float _sprintSpreadMultiplier = 1.6f;
    [SerializeField] private float _zoomSpreadMultiplier = 0.7f;
    
    [Header("Reload")]
    [SerializeField] private float _reloadTimeOnePistol = 4f;

    private int _damageLayer;
    private int _enemyLayer;
    
    private float _zoomMultiplier = 1f;
    private float _sprintMultiplier = 1f;
    
    public override void Zoom()
    {
        if(_isZooming) return;
        
        _isZooming = true;
        _zoomMultiplier = _zoomSpreadMultiplier;
        
        _audioSource.PlayOneShot(_zoomSound);
    }

    public override void UnZoom()
    {
        if(!_isZooming) return;

        _isZooming = false;
        _zoomMultiplier = 1f;
        
        _audioSource.PlayOneShot(_unzoomSound);
    }

    protected override void Hide()
    {
        _shotgun.SetActive(false);
    }
    
    public override void SetSprintSpread(bool isSprinting)
    {
        _sprintMultiplier = isSprinting ? _sprintSpreadMultiplier : 1f;
    }

    protected override void Show()
    {
        _damageLayer = LayerMask.GetMask("Damageable");
        _enemyLayer = LayerMask.GetMask("Enemies");

        _animatorHand.SetTrigger("EquipShotgun");
        _shotgun.SetActive(true);
        _animator.SetFloat("ReloadPistolSpeed", _reloadTimeOnePistol);
        _animatorHand.SetFloat("ReloadPistolSpeed", _reloadTimeOnePistol);
    }

    protected override List<RaycastHit> RaycastTarget()
    {
        List<RaycastHit> hits = new List<RaycastHit>();
        
        float effectiveSpread = _spreadAngle * _zoomMultiplier * _sprintMultiplier;
        
        for (int i = 0; i < _pelletCount; i++)
        {
            Vector3 direction;
            
            direction = GetCircularSpreadDirection(transform.forward, effectiveSpread);
            
            Ray ray = new Ray(transform.position, direction);
            
            if (Physics.Raycast(ray, out var hit1, Mathf.Infinity, _damageLayer))
            {
                hits.Add(hit1);
            }

            if (Physics.Raycast(ray, out var hit2, Mathf.Infinity, _enemyLayer, QueryTriggerInteraction.Ignore))
            {
                hits.Add(hit2);
            }
        }
        
        return hits;
    }
    
    private Vector3 GetCircularSpreadDirection(Vector3 forward, float maxAngle)
    {
        Vector2 randomPointInCircle = UnityEngine.Random.insideUnitCircle * maxAngle;
        
        Quaternion rotX = Quaternion.AngleAxis(randomPointInCircle.x, Vector3.up);
        Quaternion rotY = Quaternion.AngleAxis(randomPointInCircle.y, Vector3.right);
        
        return rotX * rotY * forward;
    }

    protected override float damageCalculation(float distance)
    {
        float distanceFactor = Mathf.Clamp01(1 - (distance / _effectiveRange));
        float damageMultiplier = distanceFactor * distanceFactor;
        
        return _pelletDamage * damageMultiplier;
    }

    protected override IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(1f);
        while (_currentCountPatron.CurrentValue < _maxCountPatron)
        {
            _animator.SetTrigger("AddPatron");
            _animatorHand.SetTrigger("AddPatron");
            
            yield return new WaitForSeconds(_reloadTimeOnePistol);
            
            if (_isReloading)
                _currentCountPatron.Value++;
        }
        
        _reloadCoroutine = null;
        _isReloading = false;
        
        _animator.SetTrigger("EndReload");
        _animatorHand.SetTrigger("EndReload");
    }
}
