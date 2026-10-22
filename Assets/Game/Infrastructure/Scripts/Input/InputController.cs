using System;
using UnityEngine;
using R3;

public class InputController
{
    public event Action ShootPressed;
    public event Action ZoomStarted;
    public event Action ZoomFinished;
    public event Action ReloadPressed;
    public event Action InteractPressed;
    public event Action InteractStopped;
    public event Action BackPressed;
    public event Action RadialMenuCanceled;
    public event Action OpenRadialMenu;
    
    public ReadOnlyReactiveProperty<float> XSensitivity => _xSensitivity;
    public ReadOnlyReactiveProperty<float> YSensitivity => _ySensitivity;
    
    public Vector2 MoveDirection => _playerInputActions.Player.Move.ReadValue<Vector2>();
    public bool SprintPressed => _playerInputActions.Player.Sprint.IsInProgress();
    
    public Vector2 RadialMenuMousePosition => _playerInputActions.RadialMenu.MousePosition.ReadValue<Vector2>();
    
    private PlayerInputActions _playerInputActions;
    
    private ReactiveProperty<float> _xSensitivity = new(4f);
    private ReactiveProperty<float> _ySensitivity = new(4f);
    
    public InputController()
    {
        _playerInputActions = new PlayerInputActions();
        
        _playerInputActions.Enable();
        this.Log("Input actions enabled");
        Subscribe();
        SwitchToGameplay();
    }

    public void SetSensitivity(float xSensitivity, float ySensitivity)
    {
        _xSensitivity.Value = xSensitivity;
        _ySensitivity.Value = ySensitivity;
    }

    public void SwitchToUi()
    {
        _playerInputActions.Player.Disable();
        _playerInputActions.RadialMenu.Disable();
        _playerInputActions.UI.Enable();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void OnOpenRadialMenu()
    {
        OpenRadialMenu?.Invoke();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnCloseRadialMenu()
    {
        RadialMenuCanceled?.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SwitchToGameplay()
    {
        _playerInputActions.UI.Disable();
        _playerInputActions.Player.Enable();
        _playerInputActions.RadialMenu.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void Enable() => _playerInputActions.Enable();
    public void Disable() => _playerInputActions.Disable();
    
    private void Subscribe()
    {
        _playerInputActions.Player.Shoot.performed += _ => ShootPressed?.Invoke();
        _playerInputActions.Player.Zoom.started += _ => ZoomStarted?.Invoke();
        _playerInputActions.Player.Zoom.canceled += _ => ZoomFinished?.Invoke();
        _playerInputActions.Player.Reload.performed += _ => ReloadPressed?.Invoke();
        _playerInputActions.Player.Interact.performed += _ => InteractPressed?.Invoke();
        _playerInputActions.Player.Interact.canceled += _ => InteractStopped?.Invoke(); 
        _playerInputActions.Player.Escape.performed += _ => BackPressed?.Invoke();
        
        _playerInputActions.UI.Back.performed += _ => BackPressed?.Invoke();
        
        _playerInputActions.RadialMenu.OpenRadialMenu.started += _ => OnOpenRadialMenu();
        _playerInputActions.RadialMenu.OpenRadialMenu.canceled += _ => OnCloseRadialMenu();
    }
}