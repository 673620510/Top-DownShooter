using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Handle : MonoBehaviour
{
    public Test1 Slide;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Fill") return;
        if (Slide.layer < collision.gameObject.GetComponent<Fill>().Slide.layer)
        {
            Slide.isDo = false;
            Slide.DoShake();
        }
    }
}
