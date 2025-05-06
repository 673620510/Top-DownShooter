using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：Boss视觉效果类
//****************************************
public class Enemy_BossVisuals : MonoBehaviour
{
    private Enemy_Boss enemy;

    [SerializeField]
    private float landingOffset = 1;//落地特效偏移量
    [SerializeField]
    private ParticleSystem landindZoneFX;//落地特效
    [SerializeField]
    private GameObject[] weaponTrails;//武器特效

    [Header("Batteries")]
    [SerializeField]
    private GameObject[] batteries;//电池
    [SerializeField]
    private float initalBatterySclaeY = .2f;//电池初始高度

    private float dischargeSpeed;//电池放电速度
    private float rechargeSpeed;//电池充电速度

    private bool isRecharging;//电池是否充电

    private void Awake()
    {
        enemy = GetComponent<Enemy_Boss>();
        landindZoneFX.transform.parent = null;
        landindZoneFX.Stop();
        ResetBatteries();
    }
    private void Update()
    {
        UpdateBatteriesScale();
    }
    /// <summary>
    /// 武器追尾特效开启状态
    /// </summary>
    /// <param name="active"></param>
    public void EnableWeaponTrail(bool active)
    {
        if (weaponTrails.Length <= 0)
        {
            Debug.LogWarning("No weapon trails found.");
            return;
        }
        foreach (var Trail in weaponTrails)
        {
            Trail.gameObject.SetActive(active);
        }
    }
    /// <summary>
    /// 设置着陆特效位置
    /// </summary>
    /// <param name="target"></param>
    public void PlaceLandindZone(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        Vector3 offset = dir.normalized * landingOffset;
        landindZoneFX.transform.position = target + offset;
        landindZoneFX.Clear();

        var mainModule = landindZoneFX.main;
        mainModule.startLifetime = enemy.travelTimeToTarger * 2;

        landindZoneFX.Play();
    }
    /// <summary>
    /// 更新电池高度
    /// </summary>
    private void UpdateBatteriesScale()
    {
        if (batteries.Length <= 0)
        {
            return;
        }
        foreach (GameObject battery in batteries)
        {
            if (battery.activeSelf)
            {
                float scaleChange = (isRecharging ? rechargeSpeed : -dischargeSpeed) * Time.deltaTime;
                float newScaleY = Mathf.Clamp(battery.transform.localScale.y + scaleChange, 0, initalBatterySclaeY);
                battery.transform.localScale = new Vector3(0.15f, newScaleY, 0.15f);
                if (battery.transform.localScale.y <= 0)
                {
                    battery.SetActive(false);
                }
            }
        }
    }
    /// <summary>
    /// 重置电池状态
    /// </summary>
    public void ResetBatteries()
    {
        isRecharging = true;
        rechargeSpeed = initalBatterySclaeY / enemy.abilityCooldown;
        dischargeSpeed = initalBatterySclaeY / (enemy.flamethrowDuration * .75f);

        foreach (GameObject battery in batteries)
        {
            battery.SetActive(true);
        }
    }
    /// <summary>
    /// 电池充电状态
    /// </summary>
    public void DischargeBatteries() => isRecharging = false;
}
