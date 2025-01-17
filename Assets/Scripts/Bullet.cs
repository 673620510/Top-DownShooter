using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//****************************************
//创建人：逸龙
//功能说明：子弹类
//****************************************
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletImpactFX;

    private Rigidbody rb => GetComponent<Rigidbody>();

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
