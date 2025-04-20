using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：近战敌人死亡状态类
//****************************************
public class DeadState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private Enemy_Ragdoll ragdoll;

    private bool interactionDisabled;//是否禁用碰撞体

    public DeadState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
        ragdoll = enemy.GetComponent<Enemy_Ragdoll>();
    }
    public override void Enter()
    {
        base.Enter();

        interactionDisabled = false;

        enemy.anim.enabled = false;
        enemy.agent.isStopped = true;

        ragdoll.RagdollActive(true);

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
            ragdoll.RagdollActive(false);
            ragdoll.CollidersActive(false);
        }
    }
}
