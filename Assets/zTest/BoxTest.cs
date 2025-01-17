using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class BoxTest : MonoBehaviour
{
    Transform targetpos;
    private void Awake()
    {
        targetpos = GameObject.Find("BoxPos").transform;
    }

    void Start()
    {
        transform.DOMove(targetpos.position, 0.5f);
        Invoke("SetSlider", 7.5f);
        Destroy(gameObject, 8);
    }
    void Update()
    {
        
    }
    void SetSlider()
    {
        GameManager.Instance.SetSlider();
    }
}
