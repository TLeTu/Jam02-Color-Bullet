using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;

    private int _currentWeaponIndex = 0;

    public Weapon CurrentWeapon => _weapons[_currentWeaponIndex];

    private void Awake()
    {
        _weapons = new List<Weapon>(GetComponentsInChildren<Weapon>());
    }

    public void FireCurrentWeapon()
    {
        CurrentWeapon.Fire(InputReader.Instance.AimPoint);
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

}
