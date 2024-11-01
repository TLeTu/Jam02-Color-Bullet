using System;
using UnityEngine;

public class EnemyCloseRangeDamageDealer : DamageDealer
{
    [SerializeField] private float _force;
    [SerializeField] private float _range;

    public void DealOneShotDamage(float dmg, float range)
    {
        _range = range;

        // overlap sphere
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _range);


        foreach (var collider in colliders)
        {
            Debug.Log(collider.name);

            if (collider.TryGetComponent<PlayerDamageReceiver>(out var receiver))
            {
                if (_hasDealDamge.Contains(receiver)) continue;

                if (receiver.IsUnvanurable) return;

                Vector2 direction = (receiver.transform.position - transform.position).normalized;
                UnitController unit = receiver.GetComponent<UnitController>();

                _hasDealDamge.Add(receiver);

                receiver.TakeDamage(dmg);
                KnockBack(unit, direction);
            }
        }


    }

    private void KnockBack(UnitController unit, Vector2 direction)
    {
        unit.OverideForce(_force, direction);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
