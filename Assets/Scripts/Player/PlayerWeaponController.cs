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

    private const float REFERNCE_BULLET_SPEED = 20;

    [SerializeField]
    private Weapon currentWeapon;
    [SerializeField]
    private Weapon secondWeapon;

    [Header("Bullet details")]
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private Transform gunPoint;

    [SerializeField]
    private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField]
    private int maxSlots = 2;
    [SerializeField]
    private List<Weapon> weaponSlosts;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
        Invoke("EquipStartingWeapon", .1f);
    }
    #region Sloats managment - Pickup\Equip\Drop Weapon
    /// <summary>
    /// 装备初始武器
    /// </summary>
    private void EquipStartingWeapon() => EquipWeapon(0);
    /// <summary>
    /// 切换武器
    /// </summary>
    /// <param name="i"></param>
    private void EquipWeapon(int i)
    {
        currentWeapon = weaponSlosts[i];
        player.weaponVisuals.PlayWeaponEquipAnimation();
    }
    /// <summary>
    /// 拾取武器
    /// </summary>
    /// <param name="newWeapon"></param>
    public void PickupWeapon(Weapon newWeapon)
    {
        if (weaponSlosts.Count >= maxSlots)
        {
            Debug.Log("No slots avalible");
            return;
        }

        weaponSlosts.Add(newWeapon);
        player.weaponVisuals.SwitchOnBackupWeaponModel();
    }
    /// <summary>
    /// 丢弃当前武器
    /// </summary>
    private void DropWeapon()
    {
        if (HasOnlyOneWeapon()) return;

        weaponSlosts.Remove(currentWeapon);

        EquipWeapon(0);
    }
    #endregion
    /// <summary>
    /// 武器开火
    /// </summary>
    private void Shoot()
    {
        if (!currentWeapon.CanShoot()) return;

        GameObject newBullet = ObjectPool.instance.GetBullet();

        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        rbNewBullet.mass = REFERNCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.velocity = BulletDirection() * bulletSpeed;

        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }
    /// <summary>
    /// 修正子弹方向
    /// </summary>
    /// <returns></returns>
    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();
        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (!player.aim.CanAimPrecisly() && player.aim.Target() == null) direction.y = 0;

        //weaponHolder.LookAt(aim);
        //gunPoint.LookAt(aim);

        return direction;
    }
    public bool HasOnlyOneWeapon() => weaponSlosts.Count <= 1;
    public Weapon CurrentWeapon() => currentWeapon;
    public Weapon BackupWeapon()
    {
        foreach (Weapon weapon in weaponSlosts)
        {
            if (weapon != currentWeapon) return weapon;
        }
        return null;
    }
    public Transform GunPoint() => gunPoint;
    #region Input Events
    /// <summary>
    /// 注册输入事件
    /// </summary>
    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;

        controls.Character.Fire.performed += context => Shoot();

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);

        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();

        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload()) player.weaponVisuals.PlayerReloadAnimation();
        };   
    }
    #endregion
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawLine(gunPoint.position, gunPoint.position + BulletDirection() * 25);
    //}
}
