using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Enemy_Boss : Enemy
{
    [Header("Boss details")]
    public float actionCooldown = 10;//动作冷却时间
    public float attackRange;//攻击范围
    [Header("Ability")]
    public ParticleSystem flamethrower;//喷火特效
    public float abilityCooldown;//技能冷却时间
    private float lastTimeUsedAbility;//上次使用技能的时间
    public float flamethrowDuration;//喷火持续时间
    public bool flamethrowActive { get; private set; }//喷火是否激活

    [Header("Jump attack")]
    public float jumpAttackCooldown = 10;//跳跃攻击冷却时间
    private float lastTimeJumped;
    public float travelTimeToTarger = 1;//跳跃攻击到达目标的时间
    public float minJumpDistanceRequired;
    [Space]
    public float impactRadius = 2.5f;//冲击半径
    public float impactPower = 5;//冲击力
    [SerializeField]
    private float upforceMultiplier = 10;//向上的冲击力
    [Space]
    [SerializeField]
    private LayerMask whatToIngore;
    public IdleState_Boss idleState { get; private set; }
    public MoveState_Boss moveState { get; private set; }
    public AttackState_Boss attackState { get; private set; }
    public JumpAttackState_Boss jumpAttackState { get; private set; }
    public AbilityState_Boss abilityState { get; private set; }

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
    }
    public void JumpImpact()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(impactPower, transform.position, impactRadius, upforceMultiplier, ForceMode.Impulse);
            }
        }
    }
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
    public bool CanDoAbility()
    {
        if (Time.time > lastTimeUsedAbility + abilityCooldown)
        {
            return true;
        }
        return false;
    }
    public void SetAbilityOnCooldown() => lastTimeUsedAbility = Time.time;
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
    public void SetJumpAttackOnCooldown() => lastTimeJumped = Time.time;
    /// <summary>
    /// 玩家是否在攻击范围内
    /// </summary>
    /// <returns></returns>
    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackRange;
}
