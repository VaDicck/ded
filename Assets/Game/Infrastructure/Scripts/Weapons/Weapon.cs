using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int _maxCountPatron = 2;
    [SerializeField] private float _fireRate = 1f;
    private int _currentCountPatron;
    private float _lastShotTime;
    
    [Header("Reload")]
    [SerializeField] private float _reloadTime;
    
    private Coroutine _reloadCoroutine;

    private void Start()
    {
        _currentCountPatron = _maxCountPatron;
        _lastShotTime = Time.time;
    }
    
    public void Shoot()
    {
        if (_currentCountPatron < 1)
        {
            Reload();
            return;
        }

        if (!(Time.time - _lastShotTime >= 1 / _fireRate)) 
            return;
        
        ReleasePatron();

        _currentCountPatron--;
            
        _lastShotTime = Time.time;
    }
    
    public void Reload()
    {
        if(_reloadCoroutine == null)
        {
            _reloadCoroutine = StartCoroutine(ReloadRoutine());
        }
        Debug.Log("Перезаряжаем");   
    }
    public void Equip()
    {
        Debug.Log("Берем");   
    }

    public void Unequip()
    {
        Debug.Log("Убираем"); 
    }

    private void ReleasePatron()
    {
        List<RaycastHit> hits = RaycastTarget();
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent<IDamageable>(out var damageableObject))
            {
                damageableObject.TakeDamage(damageCalculation(hit.distance));
            }
        }
    }
    
    protected abstract List<RaycastHit> RaycastTarget();

    protected abstract float damageCalculation(float distance);
        
    private IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(_reloadTime);
        
        _currentCountPatron =_maxCountPatron;
        
        _reloadCoroutine = null;
    }
}
