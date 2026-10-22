using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private GameObject _crosshairImage;

    private PlayerTools _playerTools;

    [Inject]
    private void Initialize(PlayerTools playerTools)
    {
        _playerTools = playerTools;
    }
    
    private void Awake()
    {
        _crosshairImage.SetActive(false);
    }
    
    private void Start()
    {
        _playerTools.ToolChanged += OnToolChanged;
    }
    
    private void Show()
    {
        _crosshairImage.SetActive(true);
    }
    
    private void Hide()
    {
        _crosshairImage.SetActive(false);
    }

    private void OnToolChanged(ToolType? tool)
    {
        if (tool == null || tool != ToolType.Shotgun)
            Hide();
        else
            Show();
    }
    
    private void OnDestroy()
    {
        _playerTools.ToolChanged -= OnToolChanged;
    }
}
