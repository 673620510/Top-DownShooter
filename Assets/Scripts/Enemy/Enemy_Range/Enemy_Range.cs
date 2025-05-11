using System.Collections.Generic;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：远程敌人类
//****************************************
public enum CoverPerk
{
    Unavalible,//不可用
    CanTakeCover,//可以使用掩体
    CanTakeAndChangeCover,//可以使用和更换掩体
}
public enum UnStoppablePerk
{
    Unavalible,//不可用
    Unstoppable,//不可阻挡
}
public enum GrenadePerk
{
    Unavalible,//不可用
    CanThrowGrenade,//可以投掷手雷
}
public class Enemy_Range : Enemy
{
    [Header("Enemy perks 敌人特性")]
    public CoverPerk coverPerk;//掩体特性
    public UnStoppablePerk unStoppablePerk;//不可阻挡特性
    public GrenadePerk grendPerk;//手雷特性

    [Header("Grenade perks 手雷特性")]
    public int grenadeDamage;//手雷伤害
    public GameObject grenadePrefab;//手雷预制体
    public float impactPower;//爆炸冲击力
    public float explosionTimer = .75f;//爆炸时间
    public float timeToTarget = 1.2f;//手雷到达目标点时间
    public float grenadeCooldown;//手雷冷却时间
    private float lastTimeGrenadeThrown = -10;//上次投掷手雷时间
    [SerializeField]
    private Transform grenadeStartPoint;//手雷起始点

    [Header("Advance perks 进攻特性")]
    public float advanceSpeed;//进攻速度
    public float advanceStoppingDistance;//进攻停止距离
    public float advanceDuration = 2.5f;//进攻时间

    [Header("Cover System 保护系统")]
    public float minCoverTime = 2;//最小停留时间
    public float safeDistance;//安全距离
    public CoverPoint currentCover{ get; private set; }//当前掩体
    public CoverPoint lastCover{ get; private set; }//上一个掩体

    [Header("Weapon details 武器细节")]
    public float attackDelay;//攻击延迟
    public Enemy_RangeWeaponType weaponType;//武器类型
    public Enemy_RangeWeaponData weaponData;//武器数据
    
    [Space]
    public Transform gunPoint;//枪口
    public Transform weaponHolder;//武器架
    public GameObject bulletPrefab;//子弹预制体

    [Header("Aim details")]
    public float slowAim = 4;//慢速瞄准
    public float fastAim = 20;//快速瞄准
    public Transform aim;
    public Transform playerBody;
    public LayerMask whatToIgnore;//忽略层

    [SerializeField]
    List<Enemy_RangeWeaponData> avalibleWeaponData;//可用武器数据
    #region States
    public IdleState_Range idleState { get; private set; }
    public MoveState_Range moveState { get; private set; }
    public BattleState_Range battleState { get; private set; }
    public RunToCoverState_Range runToCoverState { get; private set; }
    public AdvancePlayer_Range advancePlayerState { get; private set; }
    public ThrowGrenadeState_Range throwGrenadeState { get; private set; }
    public DeadState_Range deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Range(this, stateMachine, "Idle");
        moveState = new MoveState_Range(this, stateMachine, "Move");
        battleState = new BattleState_Range(this, stateMachine, "Battle");
        runToCoverState = new RunToCoverState_Range(this, stateMachine, "Run");
        advancePlayerState = new AdvancePlayer_Range(this, stateMachine, "Advance");
        throwGrenadeState = new ThrowGrenadeState_Range(this, stateMachine, "ThrowGrenade");
        deadState = new DeadState_Range(this, stateMachine, "Idle");
    }

    protected override void Start()
    {
        base.Start();

        playerBody = player.GetComponent<Player>().playerBody;
        aim.parent = null;

        InitializePerk();

        stateMachine.Initialize(idleState);
        visuals.SetupLook();
        SetupWeapon();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (player)
        {
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }
    public override void Die()
    {
        base.Die();

        if (stateMachine.currentState != deadState)
        {
            stateMachine.ChangeState(deadState);
        }
    }
    public override void EnterBattleMode()
    {
        if (inBattleMode) return;

        base.EnterBattleMode();

        if (CanGetCover())
        {
            stateMachine.ChangeState(runToCoverState);
        }
        else
        {
            stateMachine.ChangeState(battleState);
        }
    }
    protected override void InitializePerk()
    {
        if (IsUnStoppable())
        {
            advanceSpeed = 1;
            anim.SetFloat("AdvanceAnimindex", 1);
        }
        else
        {
            anim.SetFloat("AdvanceAnimindex", 0);
        }
    }
    /// <summary>
    /// 检查是否可以投掷手雷
    /// </summary>
    /// <returns></returns>
    public bool CanThrowGrenade()
    {
        if (grendPerk == GrenadePerk.Unavalible) return false;

        if (Vector3.Distance(player.position, transform.position) < safeDistance) return false;

        if (Time.time > grenadeCooldown + lastTimeGrenadeThrown) return true;

        return false;
    }
    /// <summary>
    /// 投掷手雷
    /// </summary>
    public void ThrowGrenade()
    {
        lastTimeGrenadeThrown = Time.time;
        visuals.EnableGrenadeModel(false);
        GameObject newGrenade = ObjectPool.instance.GetObject(grenadePrefab,grenadeStartPoint);
        Enemy_Grenade newGrenadeScript = newGrenade.GetComponent<Enemy_Grenade>();
        if (stateMachine.currentState == deadState)
        {
            newGrenadeScript.SetupGrenade(whatIsAlly, transform.position, 1, explosionTimer, impactPower, grenadeDamage);
            return;
        }
        newGrenadeScript.SetupGrenade(whatIsAlly, player.position, timeToTarget, explosionTimer, impactPower, grenadeDamage);
    }
    #region Cover System 掩体系统
    /// <summary>
    /// 检查是否可以使用掩体
    /// </summary>
    /// <returns></returns>
    public bool CanGetCover()
    {
        if (coverPerk == CoverPerk.Unavalible) return false;

        currentCover = AttemptToFindCover()?.GetComponent<CoverPoint>();

        if (lastCover != currentCover && currentCover != null) return true;

        Debug.Log("No cover found");

        return false;
    }
    /// <summary>
    /// 尝试寻找掩体
    /// </summary>
    /// <returns></returns>
    private Transform AttemptToFindCover()
    {
        List<CoverPoint> collectedCoverPoints = new List<CoverPoint>();
        foreach (Cover cover in CollectNearByCovers())
        {
            collectedCoverPoints.AddRange(cover.GetValidCoverPoints(transform));
        }

        CoverPoint closestCoverPoint = null;
        float shortestDistance = Mathf.Infinity;

        foreach (CoverPoint coverPoint in collectedCoverPoints)
        {
            float currentDistance = Vector3.Distance(transform.position, coverPoint.transform.position);
            if (currentDistance < shortestDistance)
            {
                closestCoverPoint = coverPoint;
                shortestDistance = currentDistance;
            }
        }
        if (closestCoverPoint != null)
        {
            lastCover?.SetOccipied(false);
            lastCover = currentCover;

            currentCover = closestCoverPoint;
            currentCover.SetOccipied(true);

            return currentCover.transform;
        }
        return null;
    }
    /// <summary>
    /// 收集附近的掩体
    /// </summary>
    /// <returns></returns>
    private List<Cover> CollectNearByCovers()
    {
        float coverCollectionRadius = 30;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, coverCollectionRadius);
        List<Cover> collectedCovers = new List<Cover>();
        foreach (Collider collider in hitColliders)
        {
            Cover cover = collider.GetComponent<Cover>();
            if (cover != null && !collectedCovers.Contains(cover))
            {
                collectedCovers.Add(cover);
            }
        }
        return collectedCovers;
    }

    #endregion
    /// <summary>
    /// 发射单发子弹
    /// </summary>
    public void FireSingleBullet()
    {
        anim.SetTrigger("Shoot");
        Vector3 bulletDirection = (aim.position - gunPoint.position).normalized;

        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab, gunPoint);
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.GetComponent<Bullet>().BulletSetUp(whatIsAlly, weaponData.bulletDamage);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Vector3 bulletDirectionWithSpread = weaponData.ApplyWeaponSpread(bulletDirection);

        rbNewBullet.mass = 20 / weaponData.bulletSpeed;
        rbNewBullet.linearVelocity = bulletDirectionWithSpread * weaponData.bulletSpeed;
    }
    /// <summary>
    /// 设置武器
    /// </summary>
    private void SetupWeapon()
    {
        List<Enemy_RangeWeaponData> filteredData = new List<Enemy_RangeWeaponData>();

        foreach (var weaponData in avalibleWeaponData)
        {
            if (weaponData.weaponType == weaponType)
            {
                filteredData.Add(weaponData);
            }
        }
        if (filteredData.Count > 0)
        {
            int randomIndex = Random.Range(0, filteredData.Count);
            weaponData = filteredData[randomIndex];
        }
        else
        {
            Debug.LogWarning("No avalible weapon data was found");
        }

        gunPoint = visuals.currentWeaponModel.GetComponent<Enemy_RangeWeaponModel>().gunPoint;
    }
    #region Enemy's aim region
    /// <summary>
    /// 更新瞄准位置
    /// </summary>
    public void UpdateAimPosition()
    {
        float aimSpeed = AimOnPlayer() ? fastAim : slowAim;
        aim.position = Vector3.MoveTowards(aim.position, playerBody.position, aimSpeed * Time.deltaTime);
    }
    /// <summary>
    /// 检查是否瞄准玩家
    /// </summary>
    /// <returns></returns>
    public bool AimOnPlayer()
    {
        float distanceAimToPlayer = Vector3.Distance(aim.position, player.position);

        return distanceAimToPlayer < 2;
    }
    /// <summary>
    /// 检查是否能看到玩家
    /// </summary>
    /// <returns></returns>
    public bool IsSeeingPlayer()
    {
        Vector3 myPosition = transform.position + Vector3.up;
        Vector3 directionToPlayer = playerBody.position - myPosition;

        if (Physics.Raycast(myPosition, directionToPlayer, out RaycastHit hit, Mathf.Infinity, ~whatToIgnore))
        {
            Debug.DrawRay(myPosition, directionToPlayer, Color.red);
            Debug.Log("Hit: " + hit.transform);
            Debug.Log("player: " + player);
            if (hit.transform == player)
            {
                UpdateAimPosition();
                return true;
            }
        }
        return false;
    }
    #endregion
    /// <summary>
    /// 检查是否不可阻挡
    /// </summary>
    /// <returns></returns>
    public bool IsUnStoppable() => unStoppablePerk == UnStoppablePerk.Unstoppable;
}
