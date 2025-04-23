using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class ThrowGrenadeState_Range : EnemyState
{
    private Enemy_Range enemy;
    public ThrowGrenadeState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.visuals.EnableWeaponModel(false);
        enemy.visuals.EnableIK(false, false);
        enemy.visuals.EnableSeconnderyWeaponModel(true);
    }

    public override void Update()
    {
        base.Update();

        Vector3 playerPos = enemy.player.position + Vector3.up;

        enemy.FaceTarget(playerPos);
        enemy.aim.position = playerPos;

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        enemy.ThrowGrenade();
    }
}
