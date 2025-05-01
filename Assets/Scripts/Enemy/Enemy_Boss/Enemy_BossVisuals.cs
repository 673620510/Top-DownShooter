using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Enemy_BossVisuals : MonoBehaviour
{
    private Enemy_Boss enemy;

    [SerializeField]
    private ParticleSystem landindZoneFX;

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
        ResetBatteries();
    }
    private void Update()
    {
        UpdateBatteriesScale();
    }
    public void PlaceLandindZone(Vector3 target)
    {
        landindZoneFX.transform.position = target;
        landindZoneFX.transform.parent = null;
        landindZoneFX.Clear();
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
