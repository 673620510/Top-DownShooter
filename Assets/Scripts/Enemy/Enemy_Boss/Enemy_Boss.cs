using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Enemy_Boss : Enemy
{
    public float attackRange;//攻击范围
    [Header("Jump attack")]
    public float jumpAttackCooldown = 10;//跳跃攻击冷却时间
    private float lastTimeJumped;
    public float travelTimeToTarger = 1;//跳跃攻击到达目标的时间
    public IdleState_Boss idleState { get; private set; }
    public MoveState_Boss moveState { get; private set; }
    public AttackState_Boss attackState { get; private set; }
    public JumpAttackState_Boss jumpAttackState { get; private set; }
    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Boss(this, stateMachine, "Idle");
        moveState = new MoveState_Boss(this, stateMachine, "Move");
        attackState = new AttackState_Boss(this, stateMachine, "Attack");
        jumpAttackState = new JumpAttackState_Boss(this, stateMachine, "JumpAttack");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(moveState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if (ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }
    }
    public override void EnterBattleMode()
    {
        base.EnterBattleMode();

        stateMachine.ChangeState(moveState);
    }
    protected override void InitializePerk()
    {
        base.InitializePerk();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    public bool CanDoJumpAttack()
    {
        if (Time.time > lastTimeJumped + jumpAttackCooldown)
        {
            lastTimeJumped = Time.time;
            return true;
        }
        return false;
    }
    /// <summary>
    /// 玩家是否在攻击范围内
    /// </summary>
    /// <returns></returns>
    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackRange;
}
