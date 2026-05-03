using System.Collections;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：限制区域类
//****************************************
public class ZoneLimitation : MonoBehaviour
{
    private ParticleSystem[] lines;//限制区域的粒子系统组件数组
    private BoxCollider zoneCollider;//限制区域的碰撞器组件

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;//隐藏限制区域的网格渲染器
        zoneCollider = GetComponent<BoxCollider>();//获取限制区域的碰撞器组件
        lines = GetComponentsInChildren<ParticleSystem>();//获取所有子对象中的粒子系统组件
        ActivateWall(false);//初始状态下停用限制区域
    }
    /// <summary>
    /// 设置限制区域的激活状态
    /// </summary>
    /// <param name="activate"></param>
    private void ActivateWall(bool activate)
    {
        foreach (var line in lines)
        {
            if (activate)
            {
                line.Play();//激活粒子系统
            }
            else
            {
                line.Stop();//停用粒子系统
            }
        }

        zoneCollider.isTrigger = !activate;
    }

    IEnumerator WallActivationCo()
    {
        ActivateWall(true);//激活限制区域

        yield return new WaitForSeconds(1f);//等待1秒

        ActivateWall(false);//停用限制区域
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(WallActivationCo());
        Debug.Log("My sensors are going crazy, I think it's dangerous");
    }
}
