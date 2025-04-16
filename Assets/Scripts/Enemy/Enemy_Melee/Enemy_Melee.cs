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
    Dodge,//翻滚
    AxeThrow//斧头投掷
}
public class Enemy_Melee : Enemy
{
    private Enemy_Visuals visuals;//敌人视觉效果类

    #region States 全部状态
    public IdleState_Melee idleState { get; private set; }
    public MoveState_Melee moveState { get; private set; }
    public RecoveryState_Melee recoveryState { get; private set; }
    public ChaseState_Melee chaseState { get; private set; }
    public AttackState_Melee attackState { get; private set; }
    public DeadState_Melee deadState { get; private set; }
    public AbilityState_Melee abilityState { get; private set; }
    #endregion

    [Header("Enemy Setting 敌人设置")]
    public EnemyMelee_Type meleeType;//敌人类型
    public Transform shieldTransform;//盾牌位置
    public float dodgeCooldown;//翻滚冷却时间
    private float lastTimeDodge = -10;//上次翻滚时间

    [Header("Axe throw Ability 斧头投掷能力")]
    public GameObject axePrefab;//斧头预制体
    public float axeFlySpeed;//斧头飞行速度
    public float axeAimTimer;//瞄准时间
    public float axeThrowCooldown;//斧头投掷冷却时间
    private float lastTimeAxeThrow;//上次投掷时间
    public Transform axeStartPoint;//斧头投掷起始位置

    [Header("Attack Data 攻击数据")]
    public AttackData attackData;//攻击数据
    public List<AttackData> attackList;//攻击列表

    protected override void Awake()
    {
        base.Awake();

        visuals = GetComponent<Enemy_Visuals>();//获取敌人视觉效果类

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

        visuals.SetupLook();
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
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackData.attackRange);
    }
    public override void EnterBattleMode()
    {
        if (inBattleMode) return;

        base.EnterBattleMode();

        stateMachine.ChangeState(recoveryState);
    }
    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        moveSpeed = moveSpeed * 0.6f;
        EnableWeaponModel(false);
    }
    /// <summary>
    /// 初始化敌人类型
    /// </summary>
    private void InitializeSpeciality()
    {
        if (meleeType == EnemyMelee_Type.AxeThrow)
        {
            visuals.SetupWeaponType(Enemy_MeleeWeaponType.Throw);
        }

        if (meleeType == EnemyMelee_Type.Shield)
        {
            anim.SetFloat("ChaseIndex", 1);
            shieldTransform.gameObject.SetActive(true);
            visuals.SetupWeaponType(Enemy_MeleeWeaponType.OneHand);
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
    /// 显示武器模型
    /// </summary>
    /// <param name="active"></param>
    public void EnableWeaponModel(bool active)
    {
        visuals.currentWeaponModel.gameObject.SetActive(true);
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

        float dodgeAnimationDuration = GetAniamtionClipDuration("Dodge roll");

        if (Time.time > dodgeCooldown + dodgeAnimationDuration + lastTimeDodge)
        {
            lastTimeDodge = Time.time;
            anim.SetTrigger("Dodge");
        }
    }
    /// <summary>
    /// 是否可以投掷斧头
    /// </summary>
    /// <returns></returns>
    public bool CanThrowAxe()
    {
        if (meleeType != EnemyMelee_Type.AxeThrow) return false;
        if (Time.time > lastTimeAxeThrow + axeThrowCooldown)
        {
            lastTimeAxeThrow = Time.time;
            return true;
        }
        return false;
    }
    /// <summary>
    /// 获取动画片段的持续时间
    /// </summary>
    /// <param name="clipName"></param>
    /// <returns></returns>
    private float GetAniamtionClipDuration(string clipName)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        Debug.LogWarning("Animation clip not found: " + clipName);
        return 0;
    }
}
