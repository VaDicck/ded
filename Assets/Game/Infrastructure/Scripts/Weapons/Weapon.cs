using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int _maxCountPatron = 2;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] protected Animator _animator;
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
    
    public bool Shoot()
    {
        if (_currentCountPatron < 1)
        {
            Reload();
            return false;
        }

        if (!(Time.time - _lastShotTime >= _fireRate)) 
            return false;
        
        _animator.SetTrigger("Shot");
        
        ReleasePatron();

        _currentCountPatron--;
            
        _lastShotTime = Time.time;
        
        return true;
    }
    
    public void Reload()
    {
        if(_reloadCoroutine == null)
        {
            _animator.SetTrigger("Reload");
            _reloadCoroutine = StartCoroutine(ReloadRoutine());
        }
    }
    
    public void Equip()
    {
        Show();
    }

    public void Unequip()
    {
        Hide();
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

    protected abstract void Hide();
    
    protected abstract void Show();
    
    protected abstract List<RaycastHit> RaycastTarget();

    protected abstract float damageCalculation(float distance);
        
    private IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(_reloadTime);
        
        _currentCountPatron =_maxCountPatron;
        
        _reloadCoroutine = null;
    }
}
