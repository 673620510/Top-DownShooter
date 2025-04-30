using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class MoveState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    private Vector3 destination;
    public MoveState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.speed = enemy.walkSpeed;
        enemy.agent.isStopped = false;

        destination = enemy.GetPatrolDestination();
        enemy.agent.SetDestination(destination);
    }

    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(GetNextPatthPoint());

        if (enemy.inBattleMode)
        {
            Vector3 playerPos = enemy.player.position;

            enemy.agent.SetDestination(playerPos);

            if (enemy.CanDoJumpAttack())
            {
                stateMachine.ChangeState(enemy.jumpAttackState);
            }
            else if (enemy.PlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (Vector3.Distance(enemy.transform.position, destination) < .25f)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
