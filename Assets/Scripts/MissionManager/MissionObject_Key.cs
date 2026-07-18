using System;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class MissionObject_Key : MonoBehaviour
{
    private GameObject player;
    public static event Action OnKeyPickedUp; // 定义钥匙拾取事件

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != player) return;
        OnKeyPickedUp?.Invoke(); // 触发钥匙拾取事件
        Destroy(gameObject); // 销毁钥匙对象
    }
}
