using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Effect : MonoBehaviour
{
    public GameObject Go;
    private void Awake()
    {

    }
    void Start()
    {
        Invoke("Create",2.3f);
        Destroy(gameObject,2.5f);
    }


    void Update()
    {
        
    }
    void Create()
    {
        Instantiate(Go,transform.position,Quaternion.identity,GameObject.Find("Canvas2").transform);
    } 
}
