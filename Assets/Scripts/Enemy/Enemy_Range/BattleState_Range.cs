using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：远程敌人战斗状态类
//****************************************
public class BattleState_Range : EnemyState
{
    private Enemy_Range enemy;

    private float lastTimeShot = -10;//上次射击时间
    private int bulletsShot = 0;//已射击子弹数量

    private int bulletsPerAttack;//每次射击子弹数量
    private float weaponCooldown;//武器冷却时间

    private float coverCheckTimer = 0;//掩体检查计时器
    private bool firstTimeAttack = true;//第一次攻击
    public BattleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        SetupValuesForFirstAttack();

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        enemy.visuals.EnableIK(true, true);

        stateTimer = enemy.attackDelay;
    }


    public override void Update()
    {
        base.Update();

        if (enemy.IsSeeingPlayer())
        {
            enemy.FaceTarget(enemy.aim.position);
        }

        if (enemy.CanThrowGrenade())
        {
            stateMachine.ChangeState(enemy.throwGrenadeState);
        }

        if (MustAdvancePlayer())
        {
            stateMachine.ChangeState(enemy.advancePlayerState);
        }

        ChangeCoverIfShould();

        if (stateTimer > 0) return;

        if (WeaponOutOfBullets())
        {
            if (enemy.IsUnStoppable() && UnStoppableWalkReady())
            {
                enemy.advanceDuration = weaponCooldown;
                stateMachine.ChangeState(enemy.advancePlayerState);
            }

            if (WeaponOnCooldown())
            {
                AttemptToResetWeapon();
            }
            return;
        }

        if (CanShoot() && enemy.AimOnPlayer())
        {
            Shoot();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
    /// <summary>
    /// 是否需要向玩家推进
    /// </summary>
    /// <returns></returns>
    private bool MustAdvancePlayer()
    {
        if (enemy.IsUnStoppable()) return false;

        return !enemy.IsPlayerInAgrresionRange() && ReadyToLeaveCover();
    }
    /// <summary>
    /// 是否准备无敌行走
    /// </summary>
    /// <returns></returns>
    private bool UnStoppableWalkReady()
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.position);
        bool outOfStoppingDistance = distanceToPlayer > enemy.advanceStoppingDistance;
        bool unstoppableWalkOnCooldown = Time.time < enemy.weaponData.minWeaponCooldown + enemy.advancePlayerState.lastTimeAdvance;
        return outOfStoppingDistance && !unstoppableWalkOnCooldown;
    }
    #region Cover system region
    /// <summary>
    /// 玩家是否靠近
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerClose()
    {
        return Vector3.Distance(enemy.player.position, enemy.transform.position) < enemy.safeDistance;
    }
    /// <summary>
    /// 是否暴露在玩家视线中
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerInClearSight()
    {
        Vector3 directionToPlayer = enemy.player.position - enemy.transform.position;
        if (Physics.Raycast(enemy.transform.position, directionToPlayer, out RaycastHit hit))
        {
            if (hit.transform.root == enemy.player.root) return true;
        }
        return false;
    }
    /// <summary>
    /// 检查是否需要更换掩体
    /// </summary>
    private void ChangeCoverIfShould()
    {
        if (enemy.coverPerk != CoverPerk.CanTakeAndChangeCover) return;

        coverCheckTimer -= Time.deltaTime;

        if (coverCheckTimer < 0)
        {
            coverCheckTimer = .5f;
            if (ReadyToChangeCover() && ReadyToLeaveCover())
            {
                if (enemy.CanGetCover())
                {
                    stateMachine.ChangeState(enemy.runToCoverState);
                }
            }
        }
    }
    /// <summary>
    /// 是否准备更换掩体
    /// </summary>
    /// <returns></returns>
    private bool ReadyToChangeCover()
    {
        bool inDanger = IsPlayerInClearSight() || IsPlayerClose();
        bool advanceTimeIsOver = Time.time > enemy.advancePlayerState.lastTimeAdvance + enemy.advanceDuration;

        return inDanger && advanceTimeIsOver;
    }
    /// <summary>
    /// 是否准备离开掩体
    /// </summary>
    /// <returns></returns>
    private bool ReadyToLeaveCover()
    {
        return Time.time > enemy.minCoverTime + enemy.runToCoverState.lastTimeTookCover;
    }
    #endregion
    #region Weapon region
    /// <summary>
    /// 重置武器状态
    /// </summary>
    private void AttemptToResetWeapon()
    {
        bulletsShot = 0;
        bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.weaponData.GetWeaponCooldown();
    }
    /// <summary>
    /// 是否武器冷却中
    /// </summary>
    /// <returns></returns>
    private bool WeaponOnCooldown() => Time.time > lastTimeShot + weaponCooldown;
    /// <summary>
    /// 是否发射完子弹
    /// </summary>
    /// <returns></returns>
    private bool WeaponOutOfBullets() => bulletsShot >= bulletsPerAttack;
    /// <summary>
    /// 是否可以射击
    /// </summary>
    /// <returns></returns>
    private bool CanShoot() => Time.time > lastTimeShot + 1 / enemy.weaponData.fireRate;
    /// <summary>
    /// 射击
    /// </summary>
    private void Shoot()
    {
        enemy.FireSingleBullet();
        lastTimeShot = Time.time;
        bulletsShot++;
    }
    /// <summary>
    /// 设置第一次攻击的武器数据
    /// </summary>
    private void SetupValuesForFirstAttack()
    {
        if (firstTimeAttack)
        {
            firstTimeAttack = false;
            bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
            weaponCooldown = enemy.weaponData.GetWeaponCooldown();
        }
    }
    #endregion
}
