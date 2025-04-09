using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：近战敌人恢复状态类
//****************************************
public class RecoveryState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    public RecoveryState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = true;
    }

    public override void Update()
    {
        base.Update();

        enemy.transform.rotation = enemy.FaceTarget(enemy.player.position);

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.chaseState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
