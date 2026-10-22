using UnityEngine;
using Unity.Cinemachine;
using Zenject;
using R3;

public class PlayerMovement : MonoBehaviour
{
    public float Speed => _currentSpeed;
    public Vector3 Velocity => _characterController.velocity;

    [Header("Menu Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CinemachineCamera _cinCam;
    [SerializeField] private CinemachineInputAxisController _inputAxisController;
    [SerializeField] private PlayerWeapons _playerWeapons;

    [SerializeField] private float _acceleration;
    [SerializeField] private float _defaultSpeed;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _sprintMultiplier;
    [SerializeField] private float _zoomXSensitivityMultiplier = 0.6f;
    [SerializeField] private float _zoomYSensitivityMultiplier = 0.6f;
    [SerializeField] private float _gravity = 9.81f;
    
    private float _verticalVelocity;

    private float _defaultXSensitivity;
    private float _defaultYSensitivity;
    private float _currentXSensitivity;
    private float _currentYSensitivity;
    
    private InputController _inputController;

    private float _currentSpeed;
    private Vector3 _velocity;

    [Inject]
    private void Init(InputController inputController, PlayerReturner returner)
    {
        _inputController = inputController;
        returner.ReturnOnSafe += ReturnOnSafe;
        
        _inputController.OpenRadialMenu += () => _inputAxisController.enabled = false;
        _inputController.RadialMenuCanceled += () => _inputAxisController.enabled = true;
        inputController.XSensitivity.Subscribe(SetDefaultXSensitivity);
        inputController.YSensitivity.Subscribe(SetDefaultYSensitivity);
    }
    
    private void Awake()
    {
        _defaultXSensitivity = _inputController.XSensitivity.CurrentValue;
        _defaultYSensitivity = _inputController.YSensitivity.CurrentValue;
            
        _currentXSensitivity = _defaultXSensitivity;
        _currentYSensitivity = _defaultYSensitivity;
        _currentSpeed = _defaultSpeed;
    }

    private void Start()
    {
        ApplySensitivity();
        _playerWeapons.IsZooming.Subscribe(Zoom);
    }

    private void Update()
    {
        Move();
    }
   
    public void SetDefaultXSensitivity(float xSensitivity)
    {
        _defaultXSensitivity = xSensitivity;
        _currentXSensitivity = _defaultXSensitivity;
        ApplySensitivity();
    }  
    
    public void SetDefaultYSensitivity(float ySensitivity)
    {
        _defaultYSensitivity = ySensitivity;
        _currentYSensitivity = _defaultYSensitivity;
        ApplySensitivity();
    }

    public void SetSensitivity(float xSensitivity, float ySensitivity)
    {
        _currentXSensitivity = Mathf.Clamp(xSensitivity, 0.1f, 50f);
        _currentYSensitivity = Mathf.Clamp(ySensitivity, 0.1f, 50f);
        ApplySensitivity();
    }

    public void SetXSensitivity(float xSensitivity)
    {
        _currentXSensitivity = Mathf.Clamp(xSensitivity, 0.1f, 50f);
        ApplySensitivity();
    }

    public void SetYSensitivity(float ySensitivity)
    {
        _currentYSensitivity = Mathf.Clamp(ySensitivity, 0.1f, 50f);
        ApplySensitivity();
    }

    public void ResetSensitivity()
    {
        _currentXSensitivity = _defaultXSensitivity;
        _currentYSensitivity = _defaultYSensitivity;
        ApplySensitivity();
    }

    private void Move()
    {
        float currentSpeed = _currentSpeed;
        Vector2 move = _inputController.MoveDirection;

        if (_inputController.SprintPressed && move.y > 0)
            currentSpeed *= _sprintMultiplier;

        if (_characterController.isGrounded && _verticalVelocity < 0)
            _verticalVelocity = 0;

        _verticalVelocity += -(_gravity) * Time.deltaTime;

        Vector3 movement = (GetForward() * move.y + GetRight() * move.x) * currentSpeed;

        movement.y = _verticalVelocity;

        _velocity = Vector3.MoveTowards(_velocity, movement, _acceleration * Time.deltaTime);

        _characterController.SimpleMove(_velocity);
    }

    private void ReturnOnSafe(Vector3 position)
    {
        _characterController.enabled = false;

        transform.position = position;
        
        _characterController.enabled = true;
    }

    private void Zoom(bool IsZoom)
    {
        if (IsZoom)
        {
            _currentSpeed = _zoomSpeed;
        
            SetSensitivity(_defaultXSensitivity*_zoomXSensitivityMultiplier,
                _defaultYSensitivity*_zoomYSensitivityMultiplier);
        }
        else
        {
            _currentSpeed = _defaultSpeed;
            ResetSensitivity();
        }
    }

    private Vector3 GetForward()
    {
        Vector3 forward = _cinCam.transform.forward;
        forward.y = 0f;

        return forward.normalized;
    }

    private Vector3 GetRight()
    {
        Vector3 right = _cinCam.transform.right;
        right.y = 0f;

        return right.normalized;
    }

    private void ApplySensitivity()
    {
        if (_inputAxisController == null) return;

        foreach (var controller in _inputAxisController.Controllers)
        {
            if (controller.Name.Contains("X") || controller.Name.Contains("Pan"))
            {
                controller.Input.Gain = _currentXSensitivity;
            }
            else if (controller.Name.Contains("Y") || controller.Name.Contains("Tilt"))
            {
                controller.Input.Gain =  -_currentYSensitivity;
            }
        }
    }
}