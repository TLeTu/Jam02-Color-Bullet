using UnityEngine;

public class ShotgunBulletDmgDealer : DamageDealer
{
    [SerializeField] private float _force;
    [SerializeField] private LayerMask _layerMask;

    private float _dmg;

    public override void DealOneShotDamage(float dmg)
    {
        base.DealOneShotDamage(dmg);

        Debug.Log("Shotgun shot");

        _hasDealDamge.Clear();
        _dmg = dmg;


    }

    private void KnockBack(DamageReceiver receiver, Vector2 direction)
    {
        UnitController unitController = receiver.GetComponentInParent<UnitController>();

        if (unitController == null) return;

        unitController.OverideForce(_force, direction);

        Debug.Log("Knockback " + unitController.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_layerMask == (_layerMask | (1 << collision.gameObject.layer)))
        {
            
            if (!collision.TryGetComponent<DamageReceiver>(out var receiver)) return;

            if (_hasDealDamge.Contains(receiver)) return;

            receiver.TakeDamage(_dmg);
        }
    }

}
