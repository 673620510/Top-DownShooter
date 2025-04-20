using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：敌人子弹类
//****************************************
public class Enemy_Bullet : Bullet
{
    protected override void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX(collision);
        ReturnBulletToPool();

        Player player = collision.gameObject.GetComponentInParent<Player>();
    }
}
