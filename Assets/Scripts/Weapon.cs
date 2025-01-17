//****************************************
//创建人：逸龙
//功能说明：武器类
//****************************************
using UnityEngine;

/// <summary>
/// 武器类型
/// </summary>
public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}
[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;

    public int bulletsInMagazine;//当前枪支弹药量
    public int magazineCapacity;//弹夹容纳量
    public int totalReserveAmmo;//当前枪支总备用弹药量

    [Range(1f, 3f)]
    public float reloadSpeed = 1;//换弹速度
    [Range(1f, 3f)]
    public float equipmentSpeed = 1;//更换装备速度

    [Space]
    public float fireRate = 1;//每秒射速

    private float lastShootTime;

    /// <summary>
    /// 是否可以射击
    /// </summary>
    /// <returns></returns>
    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }
    /// <summary>
    /// 是否准备射击
    /// </summary>
    /// <returns></returns>
    private bool ReadyToFire()
    {
        if (Time.time > lastShootTime + 1 / fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }
        return false;
    }
    #region Reload methods
    /// <summary>
    /// 是否可以换弹
    /// </summary>
    /// <returns></returns>
    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity) return false;
        if (totalReserveAmmo > 0) return true;
        return false;
    }
    /// <summary>
    /// 重新装弹
    /// </summary>
    public void RefillBullets()
    {
        //加上表示回收利用弹夹里未使用的弹药，否则表示整个弹夹丢弃
        //totalReserveAmmo += bulletsInMagazine;

        int bulletToReload = magazineCapacity;//重新装弹数量

        if (bulletToReload > totalReserveAmmo)
        {
            bulletToReload = totalReserveAmmo;
        }

        totalReserveAmmo -= bulletToReload;
        bulletsInMagazine = bulletToReload;

        if (totalReserveAmmo < 0)
        {
            totalReserveAmmo = 0;
        }
    }
    /// <summary>
    /// 是否有子弹
    /// </summary>
    /// <returns></returns>
    private bool HaveEnoughBullets()
    {
        if (bulletsInMagazine > 0)
        {
            bulletsInMagazine--;
            return true;
        }
        return false;
    }
    #endregion
}

