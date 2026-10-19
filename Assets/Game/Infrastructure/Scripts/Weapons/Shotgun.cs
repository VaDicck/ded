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

    protected override void Hide()
    {
        _shotgun.SetActive(false);
    }

    protected override void Show()
    {
        _shotgun.SetActive(true);
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
}
