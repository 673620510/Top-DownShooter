using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：近战敌人追击状态类
//****************************************
public class ChaseState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private float lastTimeUpdatedDistanation;

    public ChaseState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.speed = enemy.chaseSpeed;
        enemy.agent.isStopped = false;
    }

    public override void Update()
    {
        base.Update();

        enemy.transform.rotation = enemy.FaceTarget(GetNextPatthPoint());

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
}
