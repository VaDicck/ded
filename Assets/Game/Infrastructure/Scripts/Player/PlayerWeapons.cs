using R3;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class PlayerWeapons : MonoBehaviour
{
    public Weapon CurrentWeapon => _currentWeapon;
    public ReadOnlyReactiveProperty<bool> IsZooming => _isZooming;
    
    [SerializeField] private MonoBehaviour _weaponBehaviour;
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private CinemachinePanTilt _panTilt;
    
    [Header("Zoom")]
    [SerializeField] private float _zoomFOV = 40f;
    [SerializeField] private float _zoomSpeed = 10f;
    
    [Header("Recoil")]
    [SerializeField] private float _recoilForce = 2f;
    [SerializeField] private float _recoilReturnSpeed = 5f;
    [SerializeField] private float _recoilRecoverySpeed = 2f;
    [SerializeField] private float _recoilSnappiness = 35f;   
    [SerializeField] private float _maxRecoil = 20f;
    
    [Header("SFX")]
    [SerializeField] private GameObject _vfxFire;
    
    private float _playerInputY;
    private float _baseTilt;

    private float _recoilCurrent;
    private float _recoilTarget;
    
    private float _normalFOV;
    private float _currentFOV;
    
    private InputController _inputController;
    private PlayerTools _playerTools;
    
    private Weapon _weapon;
    private Weapon _currentWeapon;
    
    private readonly ReactiveProperty<bool> _isZooming = new(false);

    [Inject]
    private void Init(InputController inputController, PlayerTools playerTools)
    {
        _inputController = inputController;
        _playerTools = playerTools;
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
        _inputController.SprintStarted += OnSprintStarted;
        _inputController.SprintFinished += OnSprintFinished;

        _playerTools.ToolChanged += OnToolChanged;
    }
    
    private void Update()
    {
        if (_currentWeapon == null) return;
        
        UpdateZoom();
        
        _playerInputY = _panTilt.TiltAxis.Value + _recoilCurrent;
        
        UpdateRecoil();
    }

    private void OnToolChanged(ToolType? toolType)
    {
        if (toolType == ToolType.Shotgun)
        {
            if (_currentWeapon == null)
                EquipWeapon();
        }
        else
        {
            if (_currentWeapon != null)
                UnequipWeapon();
        }
    }
    
    private void UpdateRecoil()
    {
        _recoilCurrent = Mathf.Lerp(_recoilCurrent, _recoilTarget, Time.deltaTime * _recoilSnappiness);

        _recoilTarget = Mathf.MoveTowards(_recoilTarget, 0f, Time.deltaTime * _recoilRecoverySpeed);

        ApplyRecoil();
    }
    
    private void ApplyRecoil() 
    {
        var tilt = _panTilt.TiltAxis;

        tilt.Value = _playerInputY - _recoilCurrent;

        _panTilt.TiltAxis = tilt;
    }
    
    private void EquipWeapon()
    {
        if (_currentWeapon == _weapon)
        {
            this.LogWarning("Have current weapon");
            return;
        }
        
        _currentWeapon = _weapon;
        _currentWeapon.Equip();
        
        if (_inputController.SprintPressed)
            _currentWeapon.SetSprintSpread(true);
    }
    
    private void UnequipWeapon()
    {
        _vfxFire.SetActive(false); //TODO govno2.0
        
        StopZoom();
        
        _currentWeapon?.SetSprintSpread(false);
        
        _currentWeapon?.Unequip();
        _currentWeapon = null;
    }
    
    private void Shot()
    {
        if (_currentWeapon?.Shoot() == true)
        {
            _vfxFire.SetActive(false);//TODO GOVNO
            _vfxFire.SetActive(true);
            AddRecoil();
        }
    }
    
    private void AddRecoil()
    {
        _recoilTarget += _recoilForce;
        _recoilTarget = Mathf.Clamp(_recoilTarget, 0f, _maxRecoil);
    }

    private void UpdateZoom()
    {
        float currentFOV = _cinemachineCamera.Lens.FieldOfView;
        float newFOV = Mathf.Lerp(currentFOV, _currentFOV, Time.deltaTime * _zoomSpeed);
        _cinemachineCamera.Lens.FieldOfView = newFOV;
    }
    
    private void StartZoom()
    {
        if (_currentWeapon == null && _currentWeapon.IsZooming) return;
        
        _currentWeapon.Zoom();
        _isZooming.Value = _currentWeapon.IsZooming;
        _currentFOV = _zoomFOV;
    }
    
    private void StopZoom()
    {
        if (_currentWeapon == null && !_currentWeapon.IsZooming) return;
        
        _currentWeapon.UnZoom();
        _isZooming.Value = _currentWeapon.IsZooming;
        _currentFOV = _normalFOV;
    }
    
    private void OnSprintStarted()
    {
        _currentWeapon?.SetSprintSpread(true);
    }

    private void OnSprintFinished()
    {
        _currentWeapon?.SetSprintSpread(false);
    }

    private void Reload()
    {
        _currentWeapon?.Reload();
    }

    private void OnDestroy()
    {
        _inputController.ShootPressed -= Shot;
        _inputController.ZoomStarted -= StartZoom;
        _inputController.ZoomFinished -= StopZoom;
        _inputController.ReloadPressed -= Reload;
        _inputController.SprintStarted -= OnSprintStarted;
        _inputController.SprintFinished -= OnSprintFinished;

        _playerTools.ToolChanged -= OnToolChanged;
    }
}