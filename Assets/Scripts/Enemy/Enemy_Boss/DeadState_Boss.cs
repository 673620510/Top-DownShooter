using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：Boss死亡状态类
//****************************************
public class DeadState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    private bool interactionDisabled;//是否禁用碰撞体
    public DeadState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.abilityState.DisableFlamethrower();

        interactionDisabled = false;

        enemy.anim.enabled = false;
        enemy.agent.isStopped = true;

        enemy.ragdoll.RagdollActive(true);

        stateTimer = 1.5f;
    }

    public override void Update()
    {
        base.Update();

        //可选，敌人死亡后，禁用碰撞体
        //DisableInteractionIfShould();
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
