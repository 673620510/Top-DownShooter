using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//****************************************
//创建人：逸龙
//功能说明：子弹类
//****************************************
public class Bullet : MonoBehaviour
{
    private BoxCollider cd;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;

    [SerializeField]
    private GameObject bulletImpactFX;//子弹命中特效

    private Vector3 startPosition;//起始位置
    private float flyDistance;//飞行射程
    private bool bulletDisabled;//子弹是否处于禁用状态

    private void Awake()
    {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        FadeTrailIfNeeded();
        DisableBulletIfNeeded();
        ReturnTOPoolIfNeeded();
    }
    /// <summary>
    /// 必要时回收子弹进对象池
    /// </summary>
    private void ReturnTOPoolIfNeeded()
    {
        if (trailRenderer.time < 0) ObjectPool.instance.ReturnBullet(gameObject);
    }

    /// <summary>
    /// 必要时禁用子弹
    /// </summary>
    private void DisableBulletIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            cd.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }

    /// <summary>
    /// 必要时淡化弹道轨迹
    /// </summary>
    private void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
        {
            trailRenderer.time -= 2 * Time.deltaTime;
        }
    }

    /// <summary>
    /// 子弹设置
    /// </summary>
    /// <param name="flyDistance">飞行距离</param>
    public void BulletSetUp(float flyDistance)
    {
        bulletDisabled = false;
        cd.enabled = true;
        meshRenderer.enabled = true;

        trailRenderer.time = .25f;
        startPosition = transform.position;
        this.flyDistance = flyDistance + .5f;//加上0.5的射线尾端长度（PlayerAim.laserTipLenght）
    }


    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX(collision);
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        ObjectPool.instance.ReturnBullet(gameObject);
    }
    /// <summary>
    /// 生成受击特效
    /// </summary>
    /// <param name="collision"></param>
    private void CreateImpactFX(Collision collision)
    {
        //同时接触多个点位时，只获取第一个接触点生成受击特效
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            GameObject newImpactFX = Instantiate(bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(newImpactFX, 1f);
        }
    }
}
