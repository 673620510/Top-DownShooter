using UnityEngine;
using UnityEngine.AI;

//****************************************
//创建人：逸龙
//功能说明：敌人状态类
//****************************************
public class EnemyState
{
    protected Enemy enemyBase;//敌人基类
    protected EnemyStateMachine stateMachine;//状态机

    protected string animBoolName;//动画参数名称
    protected float stateTimer;//状态计时器

    protected bool triggerCalled;//是否触发完动画事件

    public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.enemyBase = enemyBase;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }
    /// <summary>
    /// 进入状态
    /// </summary>
    public virtual void Enter()
    {
        enemyBase.anim.SetBool(animBoolName, true);

        triggerCalled = false;
    }
    /// <summary>
    /// 更新状态
    /// </summary>
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    /// <summary>
    /// 退出状态
    /// </summary>
    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
    }
    /// <summary>
    /// 动画事件触发
    /// </summary>
    public void AnimationTrigger() => triggerCalled = true;
    /// <summary>
    /// 特殊能力触发
    /// </summary>
    public virtual void AbilityTrigger() { }

    /// <summary>
    /// 获取下一个路径点
    /// </summary>
    /// <returns></returns>
    protected Vector3 GetNextPatthPoint()
    {
        NavMeshAgent agent = enemyBase.agent;
        NavMeshPath path = agent.path;

        if (path.corners.Length < 2)
        {
            return agent.destination;
        }
        for (int i = 0; i < path.corners.Length; i++)
        {
            if (Vector3.Distance(agent.transform.position, path.corners[i]) < 0.1f)
            {
                return path.corners[i + 1];
            }
        }
        return agent.destination;
    }
}
