using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//****************************************
//创建人：逸龙
//功能说明：玩家武器控制器
//****************************************
public class PlayerWeaponController : MonoBehaviour
{
    private Player player;

    private const float REFERNCE_BULLET_SPEED = 20;//参考子弹速度

    [SerializeField]
    private Weapon_Data defaultWeaponData;
    [SerializeField]
    private Weapon currentWeapon;
    private bool weaponReady;
    private bool isShooting;

    [Header("Bullet details 子弹详情")]
    [SerializeField]
    private float bulletImpactForce = 100;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private Transform weaponHolder;

    [Header("Inventory 容量")]
    [SerializeField]
    private int maxSlots = 2;
    [SerializeField]
    private List<Weapon> weaponSlots;

    [SerializeField]
    private GameObject weaponPickupPrefab;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
        Invoke(nameof(EquipStartingWeapon), .1f);
    }
    private void Update()
    {
        if (isShooting) Shoot();
    }
    #region Sloats managment - Pickup\Equip\Drop\Ready Weapon
    /// <summary>
    /// 装备初始武器
    /// </summary>
    private void EquipStartingWeapon() 
    {
        weaponSlots[0] = new Weapon(defaultWeaponData);
        EquipWeapon(0);
    } 
    /// <summary>
    /// 切换武器
    /// </summary>
    /// <param name="i"></param>
    private void EquipWeapon(int i)
    {
        if (i >= weaponSlots.Count) return;

        SetWeaponReady(false);

        currentWeapon = weaponSlots[i];
        player.weaponVisuals.PlayWeaponEquipAnimation();

        CameraManager.instance.ChangeCamerDistance(currentWeapon.cameraDistance);
    }
    /// <summary>
    /// 拾取武器
    /// </summary>
    /// <param name="newWeapon"></param>
    public void PickupWeapon(Weapon newWeapon)
    {
        if (WeaponInSlots(newWeapon.weaponType) != null)
        {
            WeaponInSlots(newWeapon.weaponType).totalReserveAmmo += newWeapon.bulletsInMagazine;
            return;
        }

        if (weaponSlots.Count >= maxSlots && newWeapon.weaponType != currentWeapon.weaponType)
        {
            int weaponIndex = weaponSlots.IndexOf(currentWeapon);

            player.weaponVisuals.SwitchOffWeaponModels();
            weaponSlots[weaponIndex] = newWeapon;

            CreateWeaponOnTheGround();
            EquipWeapon(weaponIndex);
            return;
        }

        weaponSlots.Add(newWeapon);
        player.weaponVisuals.SwitchOnBackupWeaponModel();
    }
    /// <summary>
    /// 根据武器类型获取武器槽中的武器
    /// </summary>
    /// <param name="weaponType"></param>
    /// <returns></returns>
    public Weapon WeaponInSlots(WeaponType weaponType)
    {
        foreach (Weapon weapon  in weaponSlots)
        {
            if (weapon.weaponType == weaponType)
            {
                return weapon;
            }
        }
        return null;
    }

    /// <summary>
    /// 丢弃当前武器
    /// </summary>
    private void DropWeapon()
    {
        if (HasOnlyOneWeapon()) return;

        CreateWeaponOnTheGround();

        weaponSlots.Remove(currentWeapon);

        EquipWeapon(0);
    }
    /// <summary>
    /// 在地板上创建武器
    /// </summary>
    private void CreateWeaponOnTheGround()
    {
        GameObject droppedWeapon = ObjectPool.instance.GetObject(weaponPickupPrefab, transform);
        droppedWeapon.GetComponent<Pickup_Weapon>()?.SetupPickupWeapon(currentWeapon, transform);
    }

    /// <summary>
    /// 设置武器是否准备完毕
    /// </summary>
    /// <param name="ready"></param>
    public void SetWeaponReady(bool ready) => weaponReady = ready;
    /// <summary>
    /// 武器是否准备完毕
    /// </summary>
    /// <returns></returns>
    public bool WeaponReady() => weaponReady;
    #endregion
    /// <summary>
    /// 连发协程
    /// </summary>
    /// <returns></returns>
    private IEnumerator BurstFire()
    {
        SetWeaponReady(false);

        for (int i = 0; i <= currentWeapon.bulletsPerShot; i++)
        {
            FireSingleBullet();

            yield return new WaitForSeconds(currentWeapon.burstFireDelay);

            if (i >= currentWeapon.bulletsPerShot) SetWeaponReady(true);
        }
    }
    /// <summary>
    /// 武器开火
    /// </summary>
    private void Shoot()
    {
        if (!WeaponReady()) return;
        if (!currentWeapon.CanShoot()) return;

        player.weaponVisuals.PlayFireAnimation();

        if (currentWeapon.shootType == ShootType.Single) isShooting = false;
        if (currentWeapon.BurstActivated())
        {
            StartCoroutine(BurstFire());
            return;
        }

        FireSingleBullet();
        TriggerEnemyDodge();
    }
    /// <summary>
    /// 发射单发子弹
    /// </summary>
    private void FireSingleBullet()
    {
        currentWeapon.bulletsInMagazine--;

        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab, GunPoint());

        newBullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.BulletSetUp(currentWeapon.gunDistance, bulletImpactForce);

        Vector3 bulletDirection = currentWeapon.ApplySpread(BulletDirection());

        rbNewBullet.mass = REFERNCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = bulletDirection * bulletSpeed;
    }

    /// <summary>
    /// 换弹
    /// </summary>
    private void Reload()
    {
        SetWeaponReady(false);
        player.weaponVisuals.PlayerReloadAnimation();
    }
    /// <summary>
    /// 修正子弹方向
    /// </summary>
    /// <returns></returns>
    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();
        Vector3 direction = (aim.position - GunPoint().position).normalized;

        if (!player.aim.CanAimPrecisly() && player.aim.Target() == null) direction.y = 0;

        return direction;
    }
    /// <summary>
    /// 是否只有一把武器
    /// </summary>
    /// <returns></returns>
    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;
    /// <summary>
    /// 当前武器
    /// </summary>
    /// <returns></returns>
    public Weapon CurrentWeapon() => currentWeapon;
    /// <summary>
    /// 备用武器
    /// </summary>
    /// <returns></returns>
    public Weapon BackupWeapon()
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon != currentWeapon) return weapon;
        }
        return null;
    }
    /// <summary>
    /// 枪口坐标
    /// </summary>
    /// <returns></returns>
    public Transform GunPoint() => player.weaponVisuals.CurrentWeaponModel().gunPoint;
    /// <summary>
    /// 触发敌人闪避
    /// </summary>
    private void TriggerEnemyDodge()
    {
        Vector3 rayOrigin = GunPoint().position;
        Vector3 rayDirection = BulletDirection();

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, Mathf.Infinity))
        {
            Enemy_Melee enemy_Melee = hit.collider.gameObject.GetComponentInParent<Enemy_Melee>();

            if (enemy_Melee != null)
            {
                enemy_Melee.ActivateDodgeRoll();
            }
        }
    }
    #region Input Events 输入事件
    /// <summary>
    /// 注册输入事件
    /// </summary>
    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;

        controls.Character.Fire.performed += context => isShooting = true;
        controls.Character.Fire.canceled += context => isShooting = false;

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);

        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();

        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload() && WeaponReady()) Reload();
        };
        controls.Character.ToggleWeaponMode.performed += context => currentWeapon.ToggleBurst();
    }

    #endregion
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawLine(gunPoint.position, gunPoint.position + BulletDirection() * 25);
    //}
}
