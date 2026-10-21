using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [Header("Shotgun Settings")]
    [SerializeField] private float _pelletDamage = 12f;
    [SerializeField] private int _pelletCount = 8;
    [SerializeField] private float _spreadAngle = 10f;
    [SerializeField] private float _effectiveRange = 15f;
    [SerializeField] private GameObject _shotgun;
    
    [Header("Reload")]
    [SerializeField] private float _reloadTimeOnePistol = 4f;

    protected override void Hide()
    {
        _shotgun.SetActive(false);
    }

    protected override void Show()
    {
        _animatorHand.SetTrigger("EquipShotgun");
        _shotgun.SetActive(true);
        _animator.SetFloat("ReloadPistolSpeed", _reloadTimeOnePistol);
        _animatorHand.SetFloat("ReloadPistolSpeed", _reloadTimeOnePistol);
    }

    protected override List<RaycastHit> RaycastTarget()
    {
        List<RaycastHit> hits = new List<RaycastHit>();
        
        for (int i = 0; i < _pelletCount; i++)
        {
            Vector3 direction;
            
            direction = GetCircularSpreadDirection(transform.forward, _spreadAngle);
            
            Ray ray = new Ray(transform.position, direction);
            
            if (Physics.Raycast(ray, out var hit, LayerMask.GetMask("Damageable")))
            {
                hits.Add(hit);
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
