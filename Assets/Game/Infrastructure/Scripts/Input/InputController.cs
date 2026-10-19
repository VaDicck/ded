using System;
using UnityEngine;

public class InputController
{
    public event Action ShootPressed;
    public event Action ZoomStarted;
    public event Action ZoomFinished;
    public event Action ReloadPressed;
    public event Action InteractPressed;
    public event Action BackPressed;
    public event Action RadialMenuPressed;
    public event Action NextWeapon;
    public event Action RemoveWeapon;
    
    public Vector2 MoveDirection => _playerInputActions.Player.Move.ReadValue<Vector2>();
    public bool SprintPressed => _playerInputActions.Player.Sprint.IsInProgress();
    
    public Vector2 MousePosition => _playerInputActions.UI.MousePosition.ReadValue<Vector2>();
    
    private PlayerInputActions _playerInputActions;
    
    public InputController()
    {
        _playerInputActions = new PlayerInputActions();
        
        _playerInputActions.Enable();
        Subscribe();
        SwitchToGameplay();
    }

    public void SwitchToUi()
    {
        _playerInputActions.Player.Disable();
        _playerInputActions.UI.Enable();


        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void SwitchToGameplay()
    {
        _playerInputActions.UI.Disable();
        _playerInputActions.Player.Enable();

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
        _playerInputActions.Player.NextWeapon.performed += _ => NextWeapon?.Invoke();
        _playerInputActions.Player.RemoveWeapon.performed += _ => RemoveWeapon?.Invoke();
        _playerInputActions.Player.Escape.performed += _ => BackPressed?.Invoke();
        _playerInputActions.UI.Back.performed += _ => BackPressed?.Invoke();
    }
}