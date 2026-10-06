using UnityEngine;

public class Shotgun : MonoBehaviour, IWeapon
{
    public void Shoot()
    {
        Debug.Log("Стреляем");
    }

    public void Reload()
    {
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
