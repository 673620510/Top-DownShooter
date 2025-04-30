using System;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：敌人手雷类
//****************************************
public class Enemy_Grenade : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionFX;//爆炸特效
    [SerializeField]
    private float impactRadius;//爆炸半径
    [SerializeField]
    private float upwardMultiplier = 1;//向上爆炸的力
    private float impactPower;//爆炸冲击力
    private Rigidbody rb;
    private float timer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Explode();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
    /// <summary>
    /// 爆炸
    /// </summary>
    private void Explode()
    {
        GameObject newFX = ObjectPool.instance.GetObject(explosionFX, transform);

        ObjectPool.instance.ReturnObject(newFX, 1);
        ObjectPool.instance.ReturnObject(gameObject);

        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(impactPower, transform.position, impactRadius, upwardMultiplier, ForceMode.Impulse);
            }
        }
    }
    /// <summary>
    /// 设置手雷
    /// </summary>
    /// <param name="target"></param>
    /// <param name="timeToTarget"></param>
    /// <param name="countdown"></param>
    /// <param name="impactPower"></param>
    public void SetupGrenade(Vector3 target, float timeToTarget, float countdown, float impactPower)
    {
        rb.linearVelocity = CalculateLaunchVelocity(target, timeToTarget);
        timer = countdown + timeToTarget;
        this.impactPower = impactPower;
    }
    /// <summary>
    /// 计算发射速度
    /// </summary>
    /// <param name="target"></param>
    /// <param name="timeToTarget"></param>
    /// <returns></returns>
    private Vector3 CalculateLaunchVelocity(Vector3 target, float timeToTarget)
    {
        Vector3 direction = target - transform.position;
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
        Vector3 velocityXZ = directionXZ / timeToTarget;
        float velocityY = (direction.y - (Physics.gravity.y * Mathf.Pow(timeToTarget, 2)) / 2) / timeToTarget;
        Vector3 launchVelocity = velocityXZ + Vector3.up * velocityY;
        return launchVelocity;
    }
}
