using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：近战敌人追击状态类
//****************************************
public class ChaseState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private float lastTimeUpdatedDistanation;//上次更新目标位置的时间

    public ChaseState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        CheckChaseAnimation();

        base.Enter();

        enemy.agent.speed = enemy.runSpeed;
        enemy.agent.isStopped = false;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerInAttackRange())
        {
            stateMachine.ChangeState(enemy.attackState);
        }

        enemy.FaceTarget(GetNextPatthPoint());

        if (CanUpdateDestination())
        {
            enemy.agent.destination = enemy.player.transform.position;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
    /// <summary>
    /// 是否可以更新目标位置
    /// </summary>
    /// <returns></returns>
    private bool CanUpdateDestination()
    {
        if (Time.time > lastTimeUpdatedDistanation + .25f)
        {
            lastTimeUpdatedDistanation = Time.time;
            return true;
        }
        return false;
    }
    /// <summary>
    /// 检查追击动画
    /// </summary>
    private void CheckChaseAnimation()
    {
        if (enemy.meleeType == EnemyMelee_Type.Shield && enemy.shieldTransform == null)
        {
            enemy.anim.SetFloat("ChaseIndex", 0);
        }
    }
}
