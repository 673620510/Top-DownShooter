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
public class Enemy_Range : Enemy
{
    [Header("Enemy perks 敌人特性")]
    public CoverPerk coverPerk;//掩体特性

    [Header("Cover System 保护系统")]
    public float safeDistance;//安全距离
    public CoverPoint currentCover{ get; private set; }//当前掩体
    public CoverPoint lastCover{ get; private set; }//上一个掩体

    [Header("Weapon details 武器细节")]
    public Enemy_RangeWeaponType weaponType;//武器类型
    public Enemy_RangeWeaponData weaponData;//武器数据
    
    [Space]
    public Transform gunPoint;//枪口
    public Transform weaponHolder;//武器架
    public GameObject bulletPrefab;//子弹预制体

    [SerializeField]
    List<Enemy_RangeWeaponData> avalibleWeaponData;//可用武器数据
    #region States
    public IdleState_Range idleState { get; private set; }
    public MoveState_Range moveState { get; private set; }
    public BattleState_Range battleState { get; private set; }
    public RunToCoverState_Range runToCoverState { get; private set; }
    public AdvancePlayer_Range advancePlayerState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Range(this, stateMachine, "Idle");
        moveState = new MoveState_Range(this, stateMachine, "Move");
        battleState = new BattleState_Range(this, stateMachine, "Battle");
        runToCoverState = new RunToCoverState_Range(this, stateMachine, "Run");
        advancePlayerState = new AdvancePlayer_Range(this, stateMachine, "Advance");
    }

    protected override void Start()
    {
        base.Start();

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
        Vector3 bulletDirection = (player.position + Vector3.up - gunPoint.position).normalized;

        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab);
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.GetComponent<Bullet>().BulletSetUp();

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
}
