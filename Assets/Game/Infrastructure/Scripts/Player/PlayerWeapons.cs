using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public IWeapon CurrentWeapon => _currentWeapon;
    
    [SerializeField] private MonoBehaviour _weaponBehaviour;
    
    private InputController _inputController;
    
    private IWeapon _weapon;
    private IWeapon _currentWeapon;
    private int _currentIndex;

    public void Init(InputController inputController)
    {
        _inputController = inputController;
    }
    
    private void Awake()
    {
        _weapon = (IWeapon)_weaponBehaviour;
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