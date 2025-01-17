using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG;
using static UnityEditor.Progress;
//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Test : MonoBehaviour
{
    Animator animator;
    public GameObject EffectGo;
    public GameObject testGo;

    public Transform[] Rayposs;

    public int layer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void Hide()
    {
        Instantiate(EffectGo,transform.position,Quaternion.identity, transform.parent);
        GameManager.Instance.Move(gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().color);
        Destroy(gameObject, 2.3f);
    }
    public void BtnClick()
    {
        if (Test3(gameObject.transform.GetChild(0).gameObject))
        {
            Debug.Log("射线检测到被遮挡");
            GameManager.Instance.ShowOrDiable();
            return;
        }
        animator.SetTrigger("Goto");
    }
    public bool GetUIVisable(Camera cam, RectTransform ui)
    {
        bool value = true;
        Vector3 pos = cam.WorldToScreenPoint(ui.position);
        if (pos.z < 0 || pos.x < 0 || pos.x > Screen.width || pos.y < 0 || pos.y > Screen.height)
        {
            value = false;
        }
        return value;
    }
    public bool Test1()
    {
        GameObject parent = transform.parent.gameObject;
        int count = parent.transform.childCount;
        if (parent.transform.GetChild(count-1) == gameObject.transform)
        {
            return true;
        }
        return false;
    }
    public bool Test3(GameObject obj)
    {
        List<bool> bools = new List<bool>();
        foreach (var item in Rayposs)
        {
            bools.Add(Test2(item.position, obj));
        }
        foreach (var item in bools)
        {
            if (item)
            {
                return true;
            }
        }
        return false;
    }
    public bool Test2(Vector3 pos, GameObject obj)
    {
        PointerEventData saveMousePosition = new PointerEventData(EventSystem.current);
        saveMousePosition.position = pos;
        //Instantiate(testGo, pos, Quaternion.identity, transform.parent);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(saveMousePosition, results);
        if (results.Count != 0)
        {
            if (results[0].gameObject.transform.parent.parent.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;


    }
}
