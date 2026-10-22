using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private GameObject _crosshairImage;
    [SerializeField] private RectTransform _holder;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerWeapons _playerWeapons;
    [SerializeField] private float _targetSpeed;
    [SerializeField] private float _resizeMultiplier;
    [SerializeField] private float _aimReduction;
    
    private PlayerTools _playerTools;
    private float _defaultWidth;

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
        _defaultWidth = _holder.rect.width;
    }

    private void Update()
    {
        Vector2 newSize = _holder.sizeDelta;
        float x = _defaultWidth + _resizeMultiplier * _playerMovement.Velocity.magnitude / _targetSpeed;

        if (_playerWeapons.IsZooming.CurrentValue)
            x -= _aimReduction;
        
        newSize.x = x;
        _holder.sizeDelta = newSize; 
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
