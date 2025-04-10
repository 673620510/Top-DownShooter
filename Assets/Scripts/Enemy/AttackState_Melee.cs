using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：近战敌人攻击状态类
//****************************************
public class AttackState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private Vector3 attackDirection;//攻击方向

    private const float MAX_ATTACK_DISTANCE = 50f;//最大攻击距离
    public AttackState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.PullWeapon();

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, attackDirection, enemy.attackMoveSpeed * Time.deltaTime);
        }


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
