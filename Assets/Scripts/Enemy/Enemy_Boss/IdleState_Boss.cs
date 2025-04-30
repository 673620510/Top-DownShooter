using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class IdleState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    public IdleState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }
    public override void Update()
    {
        base.Update();

        if (enemy.inBattleMode && enemy.PlayerInAttackRange())
        {
            stateMachine.ChangeState(enemy.attackState);
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
