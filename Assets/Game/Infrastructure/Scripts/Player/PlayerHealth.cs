using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject[] _hitOverlays = new GameObject[5];

    private int _currentHitOverlay;
    
    public void TakeDamage(float damage, Vector3 hitPoint)
    {
        ShowRandomHitOverlay();
    }

    [ContextMenu("ShowRandomHitOverlay")]
    private void ShowRandomHitOverlay()
    {
        //int randomIndex = Random.Range(0, _hitOverlays.Length);
        if(_currentHitOverlay == _hitOverlays.Length)
            _currentHitOverlay = 0;
        
        GameObject selectedOverlay = _hitOverlays[_currentHitOverlay];
        _currentHitOverlay++;
        
        selectedOverlay.SetActive(true);
    }
}
