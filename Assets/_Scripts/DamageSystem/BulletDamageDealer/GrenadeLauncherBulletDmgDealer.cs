using System;
using UnityEngine;

public class GrenadeLauncherBulletDmgDealer : DamageDealer
{
    [SerializeField] private float _dmgRadius;
    [SerializeField] private float _force;

    public override void DealOneShotDamage(float dmg)
    {
        base.DealOneShotDamage(dmg);


        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _dmgRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out DamageReceiver receiver))
            {
                if (!_hasDealDamge.Contains(receiver))
                {
                    receiver.TakeDamage(dmg);
                    _hasDealDamge.Add(receiver);

                    Debug.Log("Deal damage to " + receiver.name);

                    KnockBack(receiver);
                }
            }
        }

    }

    private void KnockBack(DamageReceiver receiver)
    {
        UnitController unitController = receiver.GetComponentInParent<UnitController>();

        Vector2 direction = (receiver.transform.position - transform.position).normalized;

        Debug.Log("Knock back " + receiver.name + " to " + direction);


        unitController.AddForce(_force, direction);


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _dmgRadius);
    }
}
