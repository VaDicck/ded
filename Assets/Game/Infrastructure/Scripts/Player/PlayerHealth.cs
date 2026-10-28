using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject[] _hitOverlays = new GameObject[5];
    [SerializeField] private float _delayOverlay = 1f;
    
    public void TakeDamage(float damage, Vector3 hitPoint)
    {
        ShowRandomHitOverlay();
    }

    private void ShowRandomHitOverlay()
    {
        int randomIndex = Random.Range(0, _hitOverlays.Length);
        GameObject selectedOverlay = _hitOverlays[randomIndex];
        
        selectedOverlay.SetActive(true);
        
        StartCoroutine(HideOverlayAfterDelay(selectedOverlay, _delayOverlay));
    }
    
    private IEnumerator HideOverlayAfterDelay(GameObject overlay, float delay)
    {
        yield return new WaitForSeconds(delay);
        overlay.SetActive(false);
    }
}
