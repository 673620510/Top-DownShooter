using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Flamethrow_DamageArea : MonoBehaviour
{
    private Enemy_Boss enemy;
    private float damageCooldown;
    private float lastTimeDamaged;
    private int flameDamage;
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy_Boss>();
        damageCooldown = enemy.flameDamageCooldown;
        flameDamage = enemy.flameDamage;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!enemy.flamethrowActive) return;

        if (Time.time - lastTimeDamaged < damageCooldown) return;

        IDamagable damagable = other.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.TakeDamage(flameDamage);
            lastTimeDamaged = Time.time;
            damageCooldown = enemy.flameDamageCooldown;
        }
    }
}
