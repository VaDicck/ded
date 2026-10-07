using UnityEngine;
using Zenject;

public class PlayerWeapons : MonoBehaviour
{
    public Weapon CurrentWeapon => _currentWeapon;
    
    [SerializeField] private MonoBehaviour _weaponBehaviour;
    
    private InputController _inputController;
    
    private Weapon _weapon;
    private Weapon _currentWeapon;

    [Inject]
    private void Init(InputController inputController)
    {
        _inputController = inputController;
    }
    
    private void Awake()
    {
        _weapon = (Weapon)_weaponBehaviour;
    }
    
    private void Start()
    {
        _inputController.ShootPressed += Shoot;
        _inputController.ZoomPressed += Zoom;
        _inputController.ReloadPressed += Reload;
        _inputController.NextWeapon += EquipWeapon;
        _inputController.RemoveWeapon += UnequipWeapon;
    }

    private void EquipWeapon()
    {
        if(_currentWeapon == _weapon)
            return;
        
        _currentWeapon?.Unequip();

        _currentWeapon = _weapon;
        _currentWeapon.Equip();
    }
    
    private void UnequipWeapon()
    {
        _currentWeapon?.Unequip();
        _currentWeapon = null;
    }
    
    private void Shoot()
    {
        _currentWeapon?.Shoot();
    }

    private void Zoom()
    {
        
    }

    private void Reload()
    {
        _currentWeapon?.Reload();
    } 
}