using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject[] _hitOverlays = new GameObject[5];
    
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;

    private int _currentHitOverlay;
    
    public void TakeDamage(float damage, Vector3 hitPoint, AudioClip audioClip = null)
    {
        //TODO REWRITE
        if(hitPoint != Vector3.one) return;
        
        ShowRandomHitOverlay();
        
        if(audioClip == null) return;
        
        _audioSource.PlayOneShot(audioClip);
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
