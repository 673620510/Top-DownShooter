using System;
using System.Collections.Generic;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：近战敌人类
//****************************************
[Serializable]
public struct AttackData
{
    public string attackName;//攻击名称
    public float attackRange;//攻击范围
    public float moveSpeed;//攻击移动速度
    public float attackIndex;//攻击动画索引
    [Range(1, 2)]
    public float animationSpeed;//攻击动画速度
    public AttackType_Melee attackType;//攻击类型
}
public enum AttackType_Melee
{
    Close,//近战
    Charge//冲锋
}
public enum EnemyMelee_Type
{
    Regular,//普通
    Shield,//盾牌
    Dodge//翻滚
}
public class Enemy_Melee : Enemy
{
    public IdleState_Melee idleState { get; private set; }
    public MoveState_Melee moveState { get; private set; }
    public RecoveryState_Melee recoveryState { get; private set; }
    public ChaseState_Melee chaseState { get; private set; }
    public AttackState_Melee attackState { get; private set; }
    public DeadState_Melee deadState { get; private set; }
    public AbilityState_Melee abilityState { get; private set; }

    [Header("Enemy Setting 敌人设置")]
    public EnemyMelee_Type meleeType;//敌人类型
    public Transform shieldTransform;//盾牌位置
    public float dodgeCooldown;//翻滚冷却时间
    private float lastTimeDodge;//上次翻滚时间

    [Header("Attack Data 攻击数据")]
    public AttackData attackData;//攻击数据
    public List<AttackData> attackList;//攻击列表

    [SerializeField]
    private Transform hiddenWeapon;//隐藏武器
    [SerializeField]
    private Transform pulledWeapon;//拔出武器
    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Melee(this, stateMachine, "Idle");
        moveState = new MoveState_Melee(this, stateMachine, "Move");
        recoveryState = new RecoveryState_Melee(this, stateMachine, "Recovery");
        chaseState = new ChaseState_Melee(this, stateMachine, "Chase");
        attackState = new AttackState_Melee(this, stateMachine, "Attack");
        deadState = new DeadState_Melee(this, stateMachine, "Idle");//使用ragdoll
        abilityState = new AbilityState_Melee(this, stateMachine, "AxeThrow");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        InitializeSpeciality();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackData.attackRange);
    }
    public void TriggerAbility()
    {
        moveSpeed = moveSpeed * 0.6f;
        pulledWeapon.gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化敌人类型
    /// </summary>
    private void InitializeSpeciality()
    {
        if (meleeType == EnemyMelee_Type.Shield)
        {
            anim.SetFloat("ChaseIndex", 1);
            shieldTransform.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 受到伤害
    /// </summary>
    public override void GetHit()
    {
        base.GetHit();

        if (healthPoints <= 0)
        {
            stateMachine.ChangeState(deadState);
        }
    }
    /// <summary>
    /// 拿出武器
    /// </summary>
    public void PullWeapon()
    {
        hiddenWeapon.gameObject.SetActive(false);
        pulledWeapon.gameObject.SetActive(true);
    }
    /// <summary>
    /// 玩家是否在攻击范围内
    /// </summary>
    /// <returns></returns>
    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackData.attackRange;
    /// <summary>
    /// 触发翻滚
    /// </summary>
    public void ActivateDodgeRoll()
    {
        if (meleeType != EnemyMelee_Type.Dodge) return;

        if (stateMachine.currentState != chaseState) return;

        if (Vector3.Distance(transform.position, player.position) < 2f) return;

        if (Time.time > dodgeCooldown + lastTimeDodge)
        {
            lastTimeDodge = Time.time;
            anim.SetTrigger("Dodge");
        }
    }
}
