using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyController enemy, Animator animator) : base(enemy, animator)
    {
    }

    public override void OnEnter()
    {
        enemy.Attacking();
    }
}
