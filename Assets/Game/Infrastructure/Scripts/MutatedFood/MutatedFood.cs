using UnityEngine;
using R3;

public class MutatedFood : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    [SerializeField] protected GameObject _vfxSad;

    private Culture _culture;

    public void SetCulture(Culture culture)
    {
        _culture = culture;

        _culture.IsPoured.Subscribe(OnPour);
    }
    
    public void PlaceOnBed(Transform bedTransform)
    {
        transform.SetParent(bedTransform, false);
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnPour(bool value)
    {
        _vfxSad.SetActive(!value);
    }
}