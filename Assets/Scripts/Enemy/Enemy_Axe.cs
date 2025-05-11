using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：敌人投掷武器类
//****************************************
public class Enemy_Axe : MonoBehaviour
{
    [SerializeField]
    private GameObject impactFx;//命中特效
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Transform axeVisual;//斧头视觉效果

    private Vector3 direction;//投掷方向
    private Transform player;
    private float flySpeed;//飞行速度
    private float rotationSpeed;//旋转速度
    private float timer = 1;

    private int damage;

    private void FixedUpdate()
    {
        rb.linearVelocity = direction.normalized * flySpeed;
    }
    private void Update()
    {
        axeVisual.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        timer -= Time.deltaTime;

        if (timer > 0)
        {
            direction = player.position + Vector3.up - transform.position;
        }

        transform.forward = rb.linearVelocity;
    }
    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
        damagable?.TakeDamage(damage);
        GameObject newFx = ObjectPool.instance.GetObject(impactFx, transform);

        ObjectPool.instance.ReturnObject(gameObject);
        ObjectPool.instance.ReturnObject(newFx, 1);
    }
    /// <summary>
    /// 斧头设置
    /// </summary>
    /// <param name="flySpeed"></param>
    /// <param name="player"></param>
    /// <param name="timer"></param>
    public void AxeSetup(float flySpeed, Transform player, float timer, int damage)
    {
        rotationSpeed = 1600;

        this.damage = damage;
        this.flySpeed = flySpeed;
        this.player = player;
        this.timer = timer;
    }
}
