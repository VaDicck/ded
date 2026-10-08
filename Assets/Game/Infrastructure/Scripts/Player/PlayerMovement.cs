using UnityEngine;
using Unity.Cinemachine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    public float Speed => _currentSpeed;

    [Header("Menu Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CinemachineCamera _cinCam;
    [SerializeField] private CinemachineInputAxisController _inputAxisController;
    [SerializeField] private PlayerWeapons _playerWeapons;

    [SerializeField] private float _acceleration;
    [SerializeField] private float _defaultSpeed;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _sprintMultiplier;
    [SerializeField] private float _defaultXSensitivity = 1f;
    [SerializeField] private float _defaultYSensitivity = 1f;
    [SerializeField] private float _gravity = 9.81f;
    
    private float _verticalVelocity;

    private float _currentXSensitivity;
    private float _currentYSensitivity;
    
    private InputController _inputController;

    private float _currentSpeed;
    private Vector3 _velocity;

    [Inject]
    private void Init(InputController inputController)
    {
        _inputController = inputController;
    }
    
    private void Awake()
    {
        _currentXSensitivity = _defaultXSensitivity;
        _currentYSensitivity = _defaultYSensitivity;
    }

    private void Start()
    {
        ApplySensitivity();
    }

    private void Update()
    {
        Move();
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
        Vector2 move = _inputController.MoveDirection;
        _currentSpeed = _defaultSpeed;

        if (_inputController.SprintPressed && move.y > 0)
            _currentSpeed = _defaultSpeed * _sprintMultiplier;
        
        if (_playerWeapons.IsInZoom)
            _currentSpeed = _zoomSpeed;

        if (_characterController.isGrounded && _verticalVelocity < 0)
            _verticalVelocity = 0;

        _verticalVelocity += -(_gravity) * Time.deltaTime;

        Vector3 movement = (GetForward() * move.y + GetRight() * move.x) * _currentSpeed;

        movement.y = _verticalVelocity;

        _velocity = Vector3.MoveTowards(_velocity, movement, _acceleration * Time.deltaTime);

        _characterController.SimpleMove(_velocity);
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