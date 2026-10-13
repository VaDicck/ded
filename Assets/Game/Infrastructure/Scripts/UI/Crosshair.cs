using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Image _crosshairImage;
    
    private void Awake()
    {
        _crosshairImage.enabled = false;
    }
    
    public void Show()
    {
        _crosshairImage.enabled = true;
    }
    
    public void Hide()
    {
        _crosshairImage.enabled = false;
    }
}
