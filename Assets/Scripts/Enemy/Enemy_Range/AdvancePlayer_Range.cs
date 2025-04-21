using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class AdvancePlayer_Range : EnemyState
{
    private Enemy_Range enemy;
    private Vector3 playerPos;

    public float lastTimeAdvance { get; private set; }
    public AdvancePlayer_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.visuals.EnableIK(true, true);

        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.advanceSpeed;

        if (enemy.IsUnStoppable())
        {
            enemy.visuals.EnableIK(true, false);
            stateTimer = enemy.advanceDuration;
        }
    }

    public override void Update()
    {
        base.Update();

        playerPos = enemy.player.position;
        enemy.UpdateAimPosition();

        enemy.agent.SetDestination(playerPos);
        enemy.FaceTarget(GetNextPatthPoint());

        if (CanEnterBattleState() && enemy.IsSeeingPlayer())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        lastTimeAdvance = Time.time;
    }
    /// <summary>
    /// 是否可以进入战斗状态
    /// </summary>
    /// <returns></returns>
    private bool CanEnterBattleState()
    {
        bool closeEnoughToPlayer = Vector3.Distance(enemy.transform.position, playerPos) < enemy.advanceStoppingDistance && enemy.IsSeeingPlayer();
        if (enemy.IsUnStoppable())
        {
            return closeEnoughToPlayer || stateTimer < 0;
        }
        return closeEnoughToPlayer;
    }
}
