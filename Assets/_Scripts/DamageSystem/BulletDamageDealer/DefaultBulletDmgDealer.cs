using UnityEngine;

public class DefaultBulletDmgDealer : DamageDealer
{
    [SerializeField] private float _dmgRadius;
    [SerializeField] private float _force;

    public override void DealOneShotDamage(float dmg, DamageReceiver receiver)
    {
        base.DealOneShotDamage(dmg);

            if (!_hasDealDamge.Contains(receiver))
            {
                receiver.TakeDamage(dmg);
                _hasDealDamge.Add(receiver);
                //Knockback(receiver);
                //Debug.Log("Deal damage to " + receiver.name);
            }

    }
    private void Knockback(DamageReceiver receiver)
    {
        UnitController unitController = receiver.GetComponentInParent<UnitController>();

        if (unitController == null) return;

        Vector2 direction = (receiver.transform.position - transform.position).normalized;

        Debug.Log("Knock back " + receiver.name + " to " + direction);


        unitController.OverideForce(_force, direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _dmgRadius);
    }
}
