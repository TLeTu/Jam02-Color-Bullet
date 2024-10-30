using UnityEngine;
using StateMachineBehaviour;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyController enemy, Animator animator) : base(enemy, animator)
    {
    }

    public override void FixedUpdate()
    {
        enemy.HandleMovement();
    }
}
