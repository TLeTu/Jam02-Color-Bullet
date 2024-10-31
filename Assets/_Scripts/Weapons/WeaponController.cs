using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private int _currentWeaponIndex = 0;

    public Weapon CurrentWeapon => _weapons[_currentWeaponIndex];

    private void Awake()
    {
        _weapons = new List<Weapon>(GetComponentsInChildren<Weapon>());
    }

    public void FireCurrentWeapon(UnitController unit)
    {
        CurrentWeapon.Fire(InputReader.Instance.AimPoint, unit);
    }

    public void SwitchWeapon(int index)
    {
        if (index < 0 || index >= _weapons.Count)
        {
            Debug.LogWarning("Invalid weapon index");
            return;
        }

        _currentWeaponIndex = index;
    }

    public void ChangeNextWeapon()
    {
        _currentWeaponIndex = (_currentWeaponIndex + 1) % _weapons.Count;
    }

    public void ChangeWeapon(PlayerColor color)
    {
        switch (color)
        {
            case PlayerColor.White:
                SwitchWeapon(0);
                break;
            case PlayerColor.Red:
                SwitchWeapon(1);
                break;
            case PlayerColor.Orange:
                SwitchWeapon(2);
                break;
            case PlayerColor.Purple:
                SwitchWeapon(3);
                break;
            default:
                Debug.LogWarning("Invalid color");
                break;
        }
    }
}
