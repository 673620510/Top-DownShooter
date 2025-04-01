using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon System/Weapon Data")]
public class Weapon_Data : ScriptableObject//Unity提供的数据配置存储基类
{
    public string weaponName;//武器名称

    [Header("Magazine details 弹夹详情")]
    public int bulletsInMagazine;//当前枪支弹药量
    public int magazineCapacity;//弹夹容纳量
    public int totalReserveAmmo;//当前枪支总备用弹药量

    [Header("Regular shot 常规射击")]
    public ShootType shootType;//射击类型
    public int bulletPerShot = 1;//每次开火子弹数量
    public float fireRate;//每秒射速

    [Header("Burst shot 连发射击")]
    public bool burstAvalible;//是否允许连发模式
    public bool burstActive;//是否开启连发模式
    public int burstBulletsPerShot;//连发模式每次射击子弹数量
    public float burstFireRate;//连发模式射击速度
    public float burstFireDelay = .1f;//连发延迟

    [Header("Weapon spread 武器扩散")]
    public float baseSpread;//原始扩散量
    public float maxSpread;//最大扩散量
    public float SpreadIncreaseRate = .15f;//扩散增长率

    [Header("Weapon generics 武器通用")]
    public WeaponType weaponType;//武器类型
    [Range(1f, 3f)]
    public float reloadSpeed = 1;//换弹速度
    [Range(1f, 3f)]
    public float equipmentSpeed = 1;//更换装备速度
    [Range(4, 8)]
    public float gunDistance = 4;//武器射程
    [Range(4, 8)]
    public float cameraDistance = 6;//相机距离

}
