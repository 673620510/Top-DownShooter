using UnityEngine;
//****************************************
//创建人：逸龙
//功能说明：子弹类
//****************************************
public class Bullet : MonoBehaviour
{
    private float impactForce;//冲击力

    private BoxCollider cd;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;

    [SerializeField]
    private GameObject bulletImpactFX;//子弹命中特效

    private Vector3 startPosition;//起始位置
    private float flyDistance;//飞行射程
    private bool bulletDisabled;//子弹是否处于禁用状态

    private LayerMask allyLayerMask;//友军层级遮罩

    protected virtual void Awake()
    {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    protected virtual void Update()
    {
        FadeTrailIfNeeded();
        DisableBulletIfNeeded();
        ReturnTOPoolIfNeeded();
    }
    /// <summary>
    /// 必要时回收子弹进对象池
    /// </summary>
    protected void ReturnTOPoolIfNeeded()
    {
        if (trailRenderer.time < 0) ReturnBulletToPool();
    }

    /// <summary>
    /// 必要时禁用子弹
    /// </summary>
    protected void DisableBulletIfNeeded()
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
    protected void FadeTrailIfNeeded()
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
    public void BulletSetUp(LayerMask allyLayerMask, float flyDistance = 100, float impactForce = 100)
    {
        this.allyLayerMask = allyLayerMask;
        this.impactForce = impactForce;

        bulletDisabled = false;
        cd.enabled = true;
        meshRenderer.enabled = true;

        trailRenderer.Clear();
        trailRenderer.time = .25f;
        startPosition = transform.position;
        this.flyDistance = flyDistance + .5f;//加上0.5的射线尾端长度（PlayerAim.laserTipLenght）
    }


    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!FriendlyFar())
        {
            if ((allyLayerMask.value & (1 << collision.gameObject.layer)) > 0)
            {
                ReturnBulletToPool(10);
                return;
            }
        }

        CreateImpactFX();
        ReturnBulletToPool();

        IDamagable damageable = collision.gameObject.GetComponent<IDamagable>();
        damageable?.TakeDamage();

        ApplyBulletImpactToEnemy(collision);
    }

    private void ApplyBulletImpactToEnemy(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            Vector3 force = rb.linearVelocity.normalized * impactForce;
            Rigidbody hitRigidbody = collision.collider.attachedRigidbody;
            enemy.BulletImpact(force, collision.contacts[0].point, hitRigidbody);
        }
    }

    /// <summary>
    /// 回收子弹进对象池
    /// </summary>
    protected void ReturnBulletToPool(float delay = 0) => ObjectPool.instance.ReturnObject(gameObject, delay);

    /// <summary>
    /// 生成受击特效
    /// </summary>
    /// <param name="collision"></param>
    protected void CreateImpactFX()
    {
        GameObject newImpactFX = ObjectPool.instance.GetObject(bulletImpactFX, transform);
        ObjectPool.instance.ReturnObject(newImpactFX, 1);
    }
    private bool FriendlyFar() => GameManager.instance.friendlyFire;
}
