using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider _health;
    public Slider _ammo;

    public void SetMaxHealth(int health)
    {
        _health.maxValue = health;
        _health.value = health;
    }
    public void SetMaxAmmo(float ammo)
    {
        _ammo.maxValue = ammo;
        _ammo.value = ammo;
    }
    public void SetHealth(int health)
    {
        _health.value = health;
    }

    public void SetAmmo(float ammo)
    {
        _ammo.value = ammo;
    }
}
