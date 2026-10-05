using System;
using UnityEngine;

public class InputController
{
    public event Action ShootPressed;
    public event Action ZoomPressed;
    public event Action SprintPressed;
    public event Action ReloadPressed;
    public event Action InteractPressed;
    public event Action BackPressed;
    
    public Vector2 MoveDirection => _playerInputActions.Player.Move.ReadValue<Vector2>();
    
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
    }

    public void SwitchToGameplay()
    {
        _playerInputActions.UI.Disable();
        _playerInputActions.Player.Enable();
    }
    
    public void Enable() => _playerInputActions.Enable();
    public void Disable() => _playerInputActions.Disable();
    
    private void Subscribe()
    {
        _playerInputActions.Player.Shoot.performed += _ => ShootPressed?.Invoke();
        _playerInputActions.Player.Zoom.performed += _ => ZoomPressed?.Invoke();
        _playerInputActions.Player.Sprint.performed += _ => SprintPressed?.Invoke();
        _playerInputActions.Player.Reload.performed += _ => ReloadPressed?.Invoke();
        _playerInputActions.Player.Interact.performed += _ => InteractPressed?.Invoke();
        _playerInputActions.UI.Back.performed += _ => BackPressed?.Invoke();
    }
}