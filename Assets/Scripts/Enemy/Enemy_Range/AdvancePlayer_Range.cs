using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class AdvancePlayer_Range : EnemyState
{
    private Enemy_Range enemy;
    public AdvancePlayer_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
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
