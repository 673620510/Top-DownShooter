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
/// <summary>
/// 射击类型
/// </summary>
public enum ShootType
{
    Single,
    Auto
}
[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;

    [Header("Shooting specifics 射击细节")]
    public ShootType shootType;//射击类型
    public int bulletsPerShot;//每次开火子弹数量
    public float defaultFireRate;//默认每秒射速
    public float fireRate = 1;//每秒射速
    private float lastShootTime;//最后一次射击的时间

    [Header("Burst fire 连发")]
    public bool burstAvalible;//是否允许连发模式
    public bool burstActive;//是否开启连发模式
    public int burstBulletsPerShot;//连发模式每次射击子弹数量
    public float burstFireRate;//连发模式射击速度
    public float burstFireDelay = .1f;//连发延迟

    [Header("Magazine details 弹夹详情")]
    public int bulletsInMagazine;//当前枪支弹药量
    public int magazineCapacity;//弹夹容纳量
    public int totalReserveAmmo;//当前枪支总备用弹药量

    [Range(1f, 3f)]
    public float reloadSpeed = 1;//换弹速度
    [Range(1f, 3f)]
    public float equipmentSpeed = 1;//更换装备速度
    [Range(2, 12)]
    public float gunDistance = 4;//武器射程
    [Range(3, 8)]
    public float cameraDistance = 6;//相机距离

    [Header("Spread 扩散")]
    public float baseSpread = 1;//原始扩散量
    public float maximumSpread = 3;//最大扩散量
    private float currentSpread = 2;//当前扩散量

    public float SpreadIncreaseRate = .15f;//扩散增长率

    private float lastSpreadUpdateTime;//最后扩散更新时间
    private float spreadCooldown = 1;//扩散重置时间
    #region Spread methods 扩散方法们
    /// <summary>
    /// 应用扩散
    /// </summary>
    /// <param name="originalDirection">原方向</param>
    /// <returns></returns>
    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();

        float randomizedValue = Random.Range(-currentSpread, currentSpread);

        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

         return spreadRotation * originalDirection;
    }
    /// <summary>
    /// 更新扩散
    /// </summary>
    private void UpdateSpread()
    {
        if (Time.time > lastSpreadUpdateTime + spreadCooldown)
        {
            currentSpread = baseSpread;
        }
        else
        {
            IncreaseSpread();
        }

        lastSpreadUpdateTime = Time.time;
    }
    /// <summary>
    /// 扩散增长
    /// </summary>
    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + SpreadIncreaseRate, baseSpread, maximumSpread);
    }
    #endregion
    #region Burst methods
    /// <summary>
    /// 是否可以连发
    /// </summary>
    /// <returns></returns>
    public bool BurstActivated()
    {
        if (weaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;
        }
        return burstActive;
    }
    /// <summary>
    /// 连发切换器
    /// </summary>
    public void ToggleBurst()
    {
        if (!burstAvalible) return;

        burstActive = !burstActive;

        if (burstActive)
        {
            bulletsPerShot = burstBulletsPerShot;
            fireRate = burstFireRate;
        }
        else
        {
            bulletsPerShot = 1;
            fireRate = defaultFireRate;
        }

    }
    #endregion
    /// <summary>
    /// 是否可以射击
    /// </summary>
    /// <returns></returns>
    public bool CanShoot() => HaveEnoughBullets() && ReadyToFire();
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
    private bool HaveEnoughBullets() => bulletsInMagazine > 0;
    #endregion
}

