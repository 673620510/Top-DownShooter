using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：近战敌人特殊能力状态类
//****************************************
public class AbilityState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    public AbilityState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
