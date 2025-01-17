using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//****************************************
//创建人：逸龙
//功能说明：用于标记敌人类
//****************************************
[RequireComponent(typeof(Rigidbody))]
public class Target : MonoBehaviour
{
    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
}
