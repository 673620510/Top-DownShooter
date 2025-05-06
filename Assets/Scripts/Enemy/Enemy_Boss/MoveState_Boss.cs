using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：Boss移动状态类
//****************************************
public class MoveState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    private Vector3 destination;//目标点

    private float actionTimer;//动作计时器
    private float timeBeforeSpeedUp = 15;//速度提升时间

    private bool speedUpActivated;//速度提升激活状态
    public MoveState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();

        SpeedReset();
        enemy.agent.isStopped = false;

        destination = enemy.GetPatrolDestination();
        enemy.agent.SetDestination(destination);

        actionTimer = enemy.actionCooldown;
    }

    public override void Update()
    {
        base.Update();

        actionTimer -= Time.deltaTime;
        enemy.FaceTarget(GetNextPatthPoint());

        if (enemy.inBattleMode)
        {
            if (ShouldSpeedUp())
            {
                SpeedUp();
            }

            Vector3 playerPos = enemy.player.position;
            enemy.agent.SetDestination(playerPos);

            if (actionTimer < 0)
            {
                PerformRandomAction();
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
    /// <summary>
    /// 速度提升
    /// </summary>
    private void SpeedUp()
    {
        enemy.agent.speed = enemy.runSpeed;
        enemy.anim.SetFloat("MoveAnimIndex", 1);
        speedUpActivated = true;
    }
    /// <summary>
    /// 速度重置
    /// </summary>
    private void SpeedReset()
    {
        speedUpActivated = false;
        enemy.anim.SetFloat("MoveAnimIndex", 0);
        enemy.agent.speed = enemy.walkSpeed;
    }
    /// <summary>
    /// 获取随机行动
    /// </summary>
    private void PerformRandomAction()
    {
        actionTimer = enemy.actionCooldown;


        if (Random.Range(0, 2) == 0)
        {
            TryAbility();
        }
        else
        {
            if (enemy.CanDoJumpAttack())
            {
                stateMachine.ChangeState(enemy.jumpAttackState);
            }
            else if (enemy.bossWeaponType == BossWeaponType.Hummer)
            {
                TryAbility();
            }
        }
    }
    /// <summary>
    /// 尝试使用技能
    /// </summary>
    private void TryAbility()
    {
        if (enemy.CanDoAbility())
        {
            stateMachine.ChangeState(enemy.abilityState);
        }
    }
    /// <summary>
    /// 是否加速
    /// </summary>
    /// <returns></returns>
    private bool ShouldSpeedUp()
    {
        if (speedUpActivated) return false;

        if (Time.time > enemy.attackState.lastTimeAttacked + timeBeforeSpeedUp)
        {
            return true;
        }
        return false;
    }
}
