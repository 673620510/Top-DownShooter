using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Enemy_Range : Enemy
{
    public Transform weaponHolder;//武器架
    public Enemy_RangeWeaponType weaponType;//武器类型

    public float fireRate = 1f;//射击频率
    public GameObject bulletPrefab;//子弹预制体
    public Transform gunPoint;
    public float bulletSpeed = 20f;//子弹速度
    public int bulletsToShoot = 5;
    public float weaponCooldown = 1.5f;//武器冷却时间

    public IdleState_Range idleState { get; private set; }
    public MoveState_Range moveState { get; private set; }
    public BattleState_Range battleState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Range(this, stateMachine, "Idle");
        moveState = new MoveState_Range(this, stateMachine, "Move");
        battleState = new BattleState_Range(this, stateMachine, "Battle");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
        visuals.SetupLook();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }
    public override void EnterBattleMode()
    {
        if (inBattleMode) return;

        base.EnterBattleMode();
        stateMachine.ChangeState(battleState);
    }
    public void FireSingleBullet()
    {
        anim.SetTrigger("Shoot");
        Vector3 bulletDirection = (player.position + Vector3.up - gunPoint.position).normalized;

        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab);
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.GetComponent<Bullet>().BulletSetUp();

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
        rbNewBullet.mass = 20 / bulletSpeed;
        rbNewBullet.linearVelocity = bulletDirection * bulletSpeed;
    }
}
