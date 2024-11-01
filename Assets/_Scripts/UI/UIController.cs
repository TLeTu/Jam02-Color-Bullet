using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider _health;

    public void SetMaxHealth(int health)
    {
        _health.maxValue = health;
        _health.value = health;
    }
    public void SetHealth(int health)
    {
        _health.value = health;
    }
}
