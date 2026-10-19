using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class PlayerWeapons : MonoBehaviour
{
    public Weapon CurrentWeapon => _currentWeapon;
    public bool IsInZoom => _isZooming;
    
    [SerializeField] private MonoBehaviour _weaponBehaviour;
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private Crosshair _crosshair;
    
    [Header("Zoom")]
    [SerializeField] private float _zoomFOV = 40f;
    [SerializeField] private float _zoomSpeed = 10f;
    private float _normalFOV;
    private float _currentFOV;
    
    private InputController _inputController;
    
    private Weapon _weapon;
    private Weapon _currentWeapon;
    
    private bool _isZooming;

    [Inject]
    private void Init(InputController inputController)
    {
        _inputController = inputController;
    }
    
    private void Awake()
    {
        _weapon = (Weapon)_weaponBehaviour;

        _normalFOV = _cinemachineCamera.Lens.FieldOfView;
        _currentFOV = _cinemachineCamera.Lens.FieldOfView;
    }
    
    private void Start()
    {
        _inputController.ShootPressed += Shot;
        _inputController.ZoomStarted += StartZoom;
        _inputController.ZoomFinished += StopZoom;
        _inputController.ReloadPressed += Reload;
        _inputController.NextWeapon += EquipWeapon;
        _inputController.RemoveWeapon += UnequipWeapon;
    }
    
    private void Update()
    {
        UpdateZoom();
    }

    private void EquipWeapon()
    {
        _crosshair.Show();

        if (_currentWeapon == _weapon)
        {
            this.LogWarning("Have current weapon");
            return;
        }

        _currentWeapon = _weapon;
        _currentWeapon.Equip();
    }
    
    private void UnequipWeapon()
    {
        _crosshair.Hide();
        
        StopZoom();
        
        _currentWeapon?.Unequip();
        _currentWeapon = null;
    }
    
    private void Shot()
    {
        _currentWeapon?.Shoot();
    }

    private void UpdateZoom()
    {
        float currentFOV = _cinemachineCamera.Lens.FieldOfView;
        float newFOV = Mathf.Lerp(currentFOV, _currentFOV, Time.deltaTime * _zoomSpeed);
        _cinemachineCamera.Lens.FieldOfView = newFOV;
    }
    
    private void StartZoom()
    {
        if (_currentWeapon == null) return;
        
        _isZooming = true;
        _currentFOV = _zoomFOV;
    }
    
    private void StopZoom()
    {
        _isZooming = false;
        _currentFOV = _normalFOV;
    }

    private void Reload()
    {
        _currentWeapon?.Reload();
    }
}