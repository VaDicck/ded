using UnityEngine;

public class Shotgun : MonoBehaviour, IWeapon
{
    [SerializeField] private int _maxCountPatron = 2;
    private int _currentCountPatron;
    
    private void Start()
    {
        _currentCountPatron = _maxCountPatron;
    }
    
    public void Shoot()
    {
        if(_currentCountPatron < 1)
            return;

        _currentCountPatron--;
        
        Debug.Log("Стреляем");
    }

    public void Reload()
    {
        _currentCountPatron=_maxCountPatron;
        Debug.Log("Перезаряжаем");
    }

    public void Equip()
    {
        Debug.Log("Взяли");
    }

    public void Unequip()
    {
        Debug.Log("Убрали");
    }
}
