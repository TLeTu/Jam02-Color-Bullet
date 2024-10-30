using UnityEngine;

public class ShotgunBulletDmgDealer : DamageDealer
{
    [SerializeField] private float _force;
    [SerializeField] private float _range;
    [SerializeField] private float _offset;
    [SerializeField] private float _angle;
    [SerializeField] private float _rayCount;
    [SerializeField] private LayerMask _layerMask;

    public override void DealOneShotDamage(float dmg)
    {
        base.DealOneShotDamage(dmg);

        Debug.Log("Shotgun shot");

        // check collision with raycast to check in funnel
        float angle = _angle / _rayCount;
        Vector2 offsetPos = transform.position + transform.right * _offset;


        for (int i = 0; i < _rayCount; i++)
        {
            Vector2 direction = Quaternion.Euler(0, 0, angle * i - _angle / 2) * transform.right;

            RaycastHit2D hit = Physics2D.Raycast(offsetPos, direction, _range, _layerMask);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out DamageReceiver receiver))
                {
                    if (!_hasDealDamge.Contains(receiver))
                    {
                        receiver.TakeDamage(dmg);
                        _hasDealDamge.Add(receiver);

                        KnockBack(receiver, direction);
                    }
                }
            }
        }

    }

    private void KnockBack(DamageReceiver receiver, Vector2 direction)
    {
        UnitController unitController = receiver.GetComponentInParent<UnitController>();

        if (unitController == null) return;

        unitController.OverideForce(_force, direction);

        Debug.Log("Knockback " + unitController.name);
    }
}
