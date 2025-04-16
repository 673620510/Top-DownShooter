using UnityEngine;
using UnityEngine.AI;

//****************************************
//创建人：逸龙
//功能说明：近战敌人移动状态类
//****************************************
public class MoveState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private Vector3 destination;

    public MoveState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.speed = enemy.moveSpeed;

        destination = enemy.GetPatrolDestination();

        enemy.agent.SetDestination(destination);
    }
    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(GetNextPatthPoint());

        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance + .05f && !enemy.agent.pathPending)//agent.pathpending表示是否正在计算路径
        {
            stateMachine.ChangeState(enemy.idleState);            
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
