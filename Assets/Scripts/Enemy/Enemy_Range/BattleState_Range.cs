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
    public BattleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.weaponData.GetWeaponCooldown();

        enemy.visuals.EnableIK(true, true);
    }

    public override void Update()
    {
        base.Update();

        ChangeCoverIfShould();

        enemy.FaceTarget(enemy.player.position);

        if (WeaponOutOfBullets())
        {
            if (WeaponOnCooldown())
            {
                AttemptToResetWeapon();
            }
            return;
        }

        if (CanShoot())
        {
            Shoot();
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.visuals.EnableIK(false, false);
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
        if (Physics.Raycast(enemy.transform.position,directionToPlayer,out RaycastHit hit))
        {
            return hit.collider.gameObject.GetComponentInParent<Player>();
        }
        return false;
    }
    private void ChangeCoverIfShould()
    {
        if (enemy.coverPerk != CoverPerk.CanTakeAndChangeCover) return;

        coverCheckTimer -= Time.deltaTime;

        if (coverCheckTimer < 0)
        {
            coverCheckTimer = .5f;
        }

        if (IsPlayerInClearSight() || IsPlayerClose())
        {
            if (enemy.CanGetCover())
            {
                stateMachine.ChangeState(enemy.runToCoverState);
            }
        }
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
    #endregion
}
