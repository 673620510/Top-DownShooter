using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：远程敌人躲避状态类
//****************************************
public class RunToCoverState_Range : EnemyState
{
    private Enemy_Range enemy;
    private Vector3 destination;

    public float lastTimeTookCover { get; private set; }//上次掩体时间
    public RunToCoverState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        destination = enemy.currentCover.transform.position;

        enemy.visuals.EnableIK(true, false);
        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.runSpeed;

        enemy.agent.SetDestination(destination);
    }

    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(GetNextPatthPoint());

        if (Vector3.Distance(enemy.transform.position, destination) < .5f)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        lastTimeTookCover = Time.time;
    }
}
