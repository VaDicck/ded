using UnityEngine;
using Unity.Cinemachine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _defaultSpeed;
    [SerializeField] private float _sprintMultiplier;
    [SerializeField] private float _defaultXSensitivity = 1f;
    [SerializeField] private float _defaultYSensitivity = 1f;
    [SerializeField] private float _gravity = 9.81f;
    
    private float _verticalVelocity;

    private float _currentXSensitivity;
    private float _currentYSensitivity;

    private CharacterController _characterController;
    private CinemachineCamera _cinCam;
    private CinemachineInputAxisController _inputAxisController;

    private InputController _inputController;

    private bool _sprintActive;
    private float _currentSpeed;

    [Inject]
    private void Init(InputController inputController)
    {
        _inputController = inputController;
    }
    
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _cinCam = GetComponentInChildren<CinemachineCamera>();
        _inputAxisController = GetComponentInChildren<CinemachineInputAxisController>();
        
        _currentXSensitivity = _defaultXSensitivity;
        _currentYSensitivity = _defaultYSensitivity;
    }

    private void Start()
    {
        ApplySensitivity();
        
        _inputController.SprintPressed += () =>  _sprintActive = true;
        _inputController.SprintFinished += () =>  _sprintActive = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

        if (_sprintActive && move.y > 0)
            _currentSpeed = _defaultSpeed * _sprintMultiplier;
        else
            _currentSpeed = _defaultSpeed;

        if (_characterController.isGrounded && _verticalVelocity < 0)
            _verticalVelocity = 0;

        _verticalVelocity += -_gravity * Time.deltaTime;

        Vector3 movement =
            (GetForward() * move.y + GetRight() * move.x) * _currentSpeed;

        movement.y = _verticalVelocity;

        _characterController.Move(movement * Time.deltaTime);
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