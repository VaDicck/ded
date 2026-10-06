using UnityEngine;
using Unity.Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _defaultXSensitivity = 1f;
    [SerializeField] private float _defaultYSensitivity = 1f;
    //[SerializeField] private PlayerInteractions _playerInteractions;

    private float _currentXSensitivity;
    private float _currentYSensitivity;

    private CharacterController _characterController;
    private CinemachineCamera _cinCam;
    private CinemachineInputAxisController _inputAxisController;

    private PlayerInputActions _playerInputActions;

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
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        
        //_playerInteractions.Initialize(_playerInputActions);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    [ContextMenu("Enable")]
    public void Enable()
    {
        _playerInputActions.Player.Enable();
        _inputAxisController.enabled = true;
    }
    
    [ContextMenu("Disable")]
    public void Disable()
    {
        _playerInputActions?.Player.Disable();
        _inputAxisController.enabled = false;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 move = _playerInputActions.Player.Move.ReadValue<Vector2>();
        _characterController.Move( 
            (GetForward()* move.y + GetRight() * move.x) * Time.deltaTime * _currentSpeed);
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