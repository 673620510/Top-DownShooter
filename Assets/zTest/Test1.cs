using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG;
using static UnityEditor.Progress;
using DG.Tweening;
//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Test1 : MonoBehaviour
{
    //Animator animator;
    //public GameObject EffectGo;
    //public GameObject testGo;

    //public Transform[] Rayposs;

    public int layer;

    public Slider slider;
    public GameObject fill;
    public GameObject handle;
    public BoxCollider2D boxCollider;
    public Button btn;

    public float time;
    public bool isDo;

    private void Awake()
    {
        Init();
    }
    void Start()
    {
        
    }

    void Update()
    {
        boxCollider.size = new Vector2(slider.value * 400, 100);
        if (isDo)
        {
            slider.value -= Time.deltaTime * time;
        }
        if (slider.value < 0.01)
        {
            fill.SetActive(false);
            //boxCollider.enabled = false;
        }
        if (slider.value == 0)
        {
            gameObject.SetActive(false);
        }
    }
    void Init()
    {
        isDo = false;
        slider.value = 1;
        fill.SetActive(true);
        handle.SetActive(false);
    }
    public void BtnClick()
    {
        handle.SetActive(true);
        isDo = true;
    }
    public void DoShake()
    {
        gameObject.transform.DOShakePosition(1,new Vector3(3,3,0));
    }
    //public void A()
    //{
    //    slider.value = slider.gameObject.GetComponent<RectTransform>().rect.width
    //}
}
