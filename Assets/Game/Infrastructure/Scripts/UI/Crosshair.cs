using System.Collections;
using UnityEngine;
using Zenject;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private GameObject _crosshairImage;
    [SerializeField] private GameObject _hitImage;
    [SerializeField] private RectTransform _holder;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerWeapons _playerWeapons;
    [SerializeField] private float _targetSpeed;
    [SerializeField] private float _resizeMultiplier;
    [SerializeField] private float _aimReduction;
    [SerializeField] private float _hitImageDuration;
    
    private Coroutine _hitImageCoroutine;
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
        _playerWeapons.Hit += ShowHitIcon;
        _playerTools.ToolChanged += OnToolChanged;
        _defaultWidth = _holder.rect.width;
        
        _hitImage.SetActive(false);
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
    
    private void ShowHitIcon()
    {
        if (_hitImageCoroutine != null)
            StopCoroutine(_hitImageCoroutine);

        _hitImageCoroutine = StartCoroutine(ShowingHitImage());
    }

    private IEnumerator ShowingHitImage()
    {
        _hitImage.SetActive(true);
        
        yield return new WaitForSeconds(_hitImageDuration);
        
        _hitImage.SetActive(false);
        _hitImageCoroutine = null;
    }
    
    private void OnDestroy()
    {
        _playerWeapons.Hit -= ShowHitIcon;
        _playerTools.ToolChanged -= OnToolChanged;
    }
}
