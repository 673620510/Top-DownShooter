using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class DeadState_Range : EnemyState
{
    private Enemy_Range enemy;
    private bool interactionDisabled;//是否禁用碰撞体
    public DeadState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        if (!enemy.throwGrenadeState.FinishedThrowingGrenade)
        {
            enemy.ThrowGrenade();
        }
        interactionDisabled = false;

        enemy.anim.enabled = false;
        enemy.agent.isStopped = true;

        enemy.ragdoll.RagdollActive(true);

        stateTimer = 1.5f;
    }

    public override void Update()
    {
        base.Update();

        DisableInteractionIfShould();
    }

    public override void Exit()
    {
        base.Exit();
    }
    /// <summary>
    /// 禁用碰撞体
    /// </summary>
    private void DisableInteractionIfShould()
    {
        if (stateTimer < 0 && interactionDisabled == false)
        {
            interactionDisabled = true;
            enemy.ragdoll.RagdollActive(false);
            enemy.ragdoll.CollidersActive(false);
        }
    }
}
