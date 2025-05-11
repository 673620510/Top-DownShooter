using System.Collections.Generic;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：Boss类
//****************************************
/// <summary>
/// Boss武器类型
/// </summary>
public enum BossWeaponType
{
    Flamethrower,
    Hummer
}
public class Enemy_Boss : Enemy
{
    [Header("Boss details")]
    public BossWeaponType bossWeaponType;//武器类型
    public float actionCooldown = 10;//动作冷却时间
    public float attackRange;//攻击范围

    [Header("Ability")]
    public float minAbilityDistance;//技能最小距离
    public float abilityCooldown;//技能冷却时间
    private float lastTimeUsedAbility;//上次使用技能的时间

    [Header("Flamethrower")]
    public int flameDamage;//喷火伤害
    public float flameDamageCooldown;
    public ParticleSystem flamethrower;//喷火特效
    public float flamethrowDuration;//喷火持续时间
    public bool flamethrowActive { get; private set; }//喷火是否激活

    [Header("Hummer")]
    public int hummerActiveDamage;//锤子激活伤害
    public GameObject activationPrefab;//锤子特效预制体
    [SerializeField]
    private float hummerCheckRadius;

    [Header("Jump attack")]
    public int jumpAttackDamage;//跳跃攻击伤害
    public float jumpAttackCooldown = 10;//跳跃攻击冷却时间
    private float lastTimeJumped;//上次跳跃攻击的时间
    public float travelTimeToTarger = 1;//跳跃攻击到达目标的时间
    public float minJumpDistanceRequired;//跳跃攻击的最小距离
    [Space]
    public float impactRadius = 2.5f;//冲击半径
    public float impactPower = 5;//冲击力
    public Transform impactPoint;//冲击点
    [SerializeField]
    private float upforceMultiplier = 10;//向上的冲击力
    [Space]
    [SerializeField]
    private LayerMask whatToIngore;//忽略的层

    [Header("Attack")]
    [SerializeField]
    private int meleeAttackDamage;//近战攻击伤害
    [SerializeField]
    private Transform[] damagePoints;
    [SerializeField]
    private float attackCheckRadius;
    [SerializeField]
    private GameObject meleeAttackFX;//近战攻击特效

    public IdleState_Boss idleState { get; private set; }
    public MoveState_Boss moveState { get; private set; }
    public AttackState_Boss attackState { get; private set; }
    public JumpAttackState_Boss jumpAttackState { get; private set; }
    public AbilityState_Boss abilityState { get; private set; }
    public DeadState_Boss deadState { get; private set; }

    public Enemy_BossVisuals bossVisuals { get; private set; }
    protected override void Awake()
    {
        base.Awake();

        bossVisuals = GetComponent<Enemy_BossVisuals>();

        idleState = new IdleState_Boss(this, stateMachine, "Idle");
        moveState = new MoveState_Boss(this, stateMachine, "Move");
        attackState = new AttackState_Boss(this, stateMachine, "Attack");
        jumpAttackState = new JumpAttackState_Boss(this, stateMachine, "JumpAttack");
        abilityState = new AbilityState_Boss(this, stateMachine, "Ability");
        deadState = new DeadState_Boss(this, stateMachine, "Idle");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(moveState);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(abilityState);
        }

        stateMachine.currentState.Update();

        if (ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }

        MeleeAttackCheck(damagePoints, attackCheckRadius, meleeAttackFX, meleeAttackDamage);
    }
    public override void EnterBattleMode()
    {
        if (inBattleMode) return;

        base.EnterBattleMode();

        stateMachine.ChangeState(moveState);
    }
    protected override void InitializePerk()
    {
        base.InitializePerk();
    }
    public override void Die()
    {
        base.Die();

        if (stateMachine.currentState != deadState)
        {
            stateMachine.ChangeState(deadState);
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (player != null)
        {
            Vector3 myPos = transform.position + new Vector3(0, 1.5f, 0);
            Vector3 playerPos = player.position + Vector3.up;

            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(myPos, playerPos);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minJumpDistanceRequired);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, impactRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, minAbilityDistance);

        if (damagePoints.Length > 0)
        {
            foreach (var damagePoint in damagePoints)
            {
                Gizmos.DrawWireSphere(damagePoint.position, attackCheckRadius);
            }
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(damagePoints[0].position, hummerCheckRadius);
        }
    }
    /// <summary>
    /// 施加跳跃冲击力
    /// </summary>
    public void JumpImpact()
    {
        Transform impactPoint = this.impactPoint;
        if (impactPoint != null)
        {
            impactPoint = transform;
        }
        MassDamage(impactPoint.position, impactRadius, jumpAttackDamage);
    }
    private void MassDamage(Vector3 impactPoint, float impactRadius, int damage)
    {
        HashSet<GameObject> uniqueEntities = new HashSet<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(impactPoint, impactRadius, ~whatIsAlly);
        foreach (Collider hit in colliders)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if (damagable != null)
            {
                GameObject rootEntity = hit.transform.root.gameObject;

                if (!uniqueEntities.Add(rootEntity)) continue;

                damagable.TakeDamage(damage);
            }
            ApplyPhysicalForceTo(impactPoint, impactRadius, hit);
        }
    }

    private void ApplyPhysicalForceTo(Vector3 impactPoint, float impactRadius, Collider hit)
    {
        Rigidbody rb = hit.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddExplosionForce(impactPower, impactPoint, impactRadius, upforceMultiplier, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// 喷火器激活状态
    /// </summary>
    /// <param name="activate"></param>
    public void ActivateFlamethrower(bool activate)
    {
        flamethrowActive = activate;
        if (!activate)
        {
            flamethrower.Stop();
            anim.SetTrigger("StopFlamethrower");
            return;
        }
        var mainModule = flamethrower.main;
        var extraModule = flamethrower.transform.GetChild(0).GetComponent<ParticleSystem>().main;

        mainModule.duration = flamethrowDuration;
        extraModule.duration = flamethrowDuration;

        flamethrower.Clear();
        flamethrower.Play();

    }
    /// <summary>
    /// 锤子激活状态
    /// </summary>
    public void ActivateHummer()
    {
        GameObject newActivation = ObjectPool.instance.GetObject(activationPrefab, impactPoint);

        ObjectPool.instance.ReturnObject(newActivation, 1);

        MassDamage(damagePoints[0].position, hummerCheckRadius, hummerActiveDamage);
    }
    /// <summary>
    /// 是否可以施加技能
    /// </summary>
    /// <returns></returns>
    public bool CanDoAbility()
    {
        bool playerWithinDistance = Vector3.Distance(transform.position, player.position) < minAbilityDistance;

        if (!playerWithinDistance) return false;

        if (Time.time > lastTimeUsedAbility + abilityCooldown && playerWithinDistance)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 设置技能冷却时间
    /// </summary>
    public void SetAbilityOnCooldown() => lastTimeUsedAbility = Time.time;
    /// <summary>
    /// 玩家是否在视线范围内
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerInClearSight()
    {
        Vector3 myPos = transform.position + new Vector3(0, 1.5f, 0);
        Vector3 playerPos = player.position + Vector3.up;
        Vector3 directionToPlayer = (playerPos - myPos).normalized;

        if (Physics.Raycast(myPos, directionToPlayer, out RaycastHit hit, 100, ~whatToIngore))
        {
            if (hit.transform == player || hit.transform.parent == player)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 是否可以施加跳跃攻击
    /// </summary>
    /// <returns></returns>
    public bool CanDoJumpAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < minJumpDistanceRequired) return false;

        if (Time.time > lastTimeJumped + jumpAttackCooldown && IsPlayerInClearSight())
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 设置跳跃攻击冷却时间
    /// </summary>
    public void SetJumpAttackOnCooldown() => lastTimeJumped = Time.time;
    /// <summary>
    /// 玩家是否在攻击范围内
    /// </summary>
    /// <returns></returns>
    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackRange;
}
