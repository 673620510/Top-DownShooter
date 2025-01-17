using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {  get { return instance; } }
    public GameObject prefabGo;
    public Transform pos;
    public Transform boxpos;
    public Text warringText;
    public Slider slider;
    GameObject[] gos;//游戏开始的卷纸
    public float overCount = 0;
    List<GameObject> goList = new List<GameObject>();

    float timer;

    private void Awake()
    {
        instance = this;
        gos = GameObject.FindGameObjectsWithTag("卷纸");
        slider.value = 0;
    }
    void Start()
    {
        
        foreach (var item in gos)
        {
            Color color = item.transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
            for (int i = 0; i < 5; i++)
            {
                GameObject go = Instantiate(prefabGo, pos.position, Quaternion.identity, GameObject.Find("Canvas").transform);
                goList.Add(go);
                go.GetComponent<Image>().color = color;

            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
    }
    public void Move(Color color)
    {
        float time = 3f;
        foreach(var item in goList)
        {
            if (item.GetComponent<Image>().color == color)
            {
                StartCoroutine(test(item, time++));
            }
        }
    }
    IEnumerator test(GameObject item , float time)
    {
        yield return new WaitForSeconds(time);
        item.transform.DOMove(boxpos.position, 2);
        goList.Remove(item);
        Destroy(item,3);
    }
    public void ShowOrDiable()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(warringText.GetComponent<CanvasGroup>().DOFade(1, 1f));
        seq.Append(warringText.GetComponent<CanvasGroup>().DOFade(0, 1f));
    }
    public void SetSlider()
    {
        overCount++;
        slider.value = overCount / gos.Length;
    }
}
