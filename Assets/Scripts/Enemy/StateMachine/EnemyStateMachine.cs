//****************************************
//创建人：逸龙
//功能说明：敌人状态机类
//****************************************

using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState { get; private set; }//当前状态
    /// <summary>
    /// 初始化状态
    /// </summary>
    /// <param name="startState"></param>
    public void Initialize(EnemyState startState)
    {
        currentState = startState;
        currentState.Enter();
    }
    /// <summary>
    /// 改变状态
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(EnemyState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
