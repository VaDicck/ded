using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot
{
    private IWeapon _weapon;
    private InputController _inputController;
    
    public void Init(InputController inputController)
    {
        _inputController = inputController;
    }

    private void Start()
    {
        _inputController.ShootPressed += Shoot;
        _inputController.ZoomPressed += Zoom;
        _inputController.ReloadPressed += Reload;
    }

    private void Shoot()
    {
        _weapon.Shoot();
    }

    private void Zoom()
    {
        
    }

    private void Reload()
    {
        _weapon.Reload();
    }
}
