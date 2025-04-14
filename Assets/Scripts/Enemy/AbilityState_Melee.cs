using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：近战敌人特殊能力状态类
//****************************************
public class AbilityState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private Vector3 movementDirection;//攻击方向
    private const float MAX_MOVEMENT_DISTANCE = 20f;//最大攻击距离

    private float moveSpeed;

    public AbilityState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }
    public override void Enter()
    {
        base.Enter();

        moveSpeed = enemy.moveSpeed;
        movementDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);
    }
    public override void Update()
    {
        base.Update();

        if (enemy.ManualRotationActive())
        {
            enemy.transform.rotation = enemy.FaceTarget(enemy.player.position);
            movementDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);
        }

        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, movementDirection, enemy.moveSpeed * Time.deltaTime);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.recoveryState);
        }
    }
    public override void Exit()
    {
        base.Exit();

        enemy.moveSpeed = moveSpeed;
        enemy.anim.SetFloat("RecoveryIndex", 0);
    }
}
