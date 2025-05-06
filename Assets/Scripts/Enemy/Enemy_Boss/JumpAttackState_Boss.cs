using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：Boss跳跃攻击状态类
//****************************************
public class JumpAttackState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    private Vector3 lastPlayerPos;//上次玩家位置

    private float jumpAttatckMovementSpeed;//跳跃攻击移动速度
    public JumpAttackState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();

        lastPlayerPos = enemy.player.position;
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        enemy.bossVisuals.PlaceLandindZone(lastPlayerPos);
        enemy.bossVisuals.EnableWeaponTrail(true);

        float distanceToPlayer = Vector3.Distance(lastPlayerPos, enemy.transform.position);

        jumpAttatckMovementSpeed = distanceToPlayer / enemy.travelTimeToTarger;

        enemy.FaceTarget(lastPlayerPos, 1000);

        if (enemy.bossWeaponType == BossWeaponType.Hummer)
        {
            enemy.agent.isStopped = false;
            enemy.agent.speed = enemy.runSpeed;
            enemy.agent.SetDestination(lastPlayerPos);
        }
    }

    public override void Update()
    {
        base.Update();

        Vector3 myPos = enemy.transform.position;
        enemy.agent.enabled = !enemy.ManualMovementActive();

        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(myPos, lastPlayerPos, jumpAttatckMovementSpeed * Time.deltaTime);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.SetJumpAttackOnCooldown();
        enemy.bossVisuals.EnableWeaponTrail(false);
    }
}
