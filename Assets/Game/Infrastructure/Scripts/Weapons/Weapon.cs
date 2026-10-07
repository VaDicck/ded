using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int _maxCountPatron = 2;
    private int _currentCountPatron;

    private void Start()
    {
        _currentCountPatron = _maxCountPatron;
    }

    public void Shoot()
    {
        if (_currentCountPatron < 1)
            return;

        _currentCountPatron--;
    }
    public void Reload()
    {
        _currentCountPatron = _maxCountPatron;
    }
    public void Equip()
    {

    }

    public void Unequip()
    {

    }
}
