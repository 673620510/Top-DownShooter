using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Enemy_BossVisuals : MonoBehaviour
{
    private Enemy_Boss enemy;

    [SerializeField]
    private float landingOffset = 1;
    [SerializeField]
    private ParticleSystem landindZoneFX;
    [SerializeField]
    private GameObject[] weaponTrails;

    [Header("Batteries")]
    [SerializeField]
    private GameObject[] batteries;
    [SerializeField]
    private float initalBatterySclaeY = .2f;

    private float dischargeSpeed;//电池放电速度
    private float rechargeSpeed;//电池充电速度

    private bool isRecharging;

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
    public void DischargeBatteries() => isRecharging = false;
}
