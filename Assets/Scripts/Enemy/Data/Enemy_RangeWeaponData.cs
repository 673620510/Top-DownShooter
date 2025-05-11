using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：远程敌人武器数据类
//****************************************
[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data/Range Weapon Data")]
public class Enemy_RangeWeaponData : ScriptableObject
{
    [Header("Weapon details 武器详情")]
    public Enemy_RangeWeaponType weaponType; //远程敌人武器类型
    public float fireRate = 1f;//射速

    public int minBulletsPerAttack = 1;//最小攻击子弹数
    public int maxBulletsPerAttack = 1;//最大攻击子弹数

    public float minWeaponCooldown = 2;//最小武器冷却时间
    public float maxWeaponCooldown = 3;//最大武器冷却时间

    [Header("Bullet details 子弹详情")]
    public int bulletDamage;
    [Space]
    public float bulletSpeed = 20;//子弹速度
    public float weaponSpread = .1f;//扩撒范围

    /// <summary>
    /// 获取子弹数量
    /// </summary>
    /// <returns></returns>
    public int GetBulletsPerAttack() => Random.Range(minBulletsPerAttack, maxBulletsPerAttack + 1);
    /// <summary>
    /// 获取武器冷却时间
    /// </summary>
    /// <returns></returns>
    public float GetWeaponCooldown() => Random.Range(minWeaponCooldown, maxWeaponCooldown + 1);
    /// <summary>
    /// 应用武器扩撒
    /// </summary>
    /// <param name="originalDirection">原方向</param>
    /// <returns></returns>
    public Vector3 ApplyWeaponSpread(Vector3 originalDirection)
    {
        float randomizedValue = Random.Range(-weaponSpread, weaponSpread);

        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

        return spreadRotation * originalDirection;
    }
}
