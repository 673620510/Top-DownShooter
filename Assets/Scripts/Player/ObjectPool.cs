using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//****************************************
//创建人：逸龙
//功能说明：对象池类
//****************************************
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField]
    private GameObject bulletPrefab;//对象池预制体
    [SerializeField]
    private int poolSize = 10;//对象池容量

    private Queue<GameObject> bulletPool;//对象池队列

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        bulletPool = new Queue<GameObject>();
        CreateInitialPool();        
    }
    /// <summary>
    /// 获取对象池中的子弹
    /// </summary>
    /// <returns></returns>
    public GameObject GetBullet()
    {
        if (bulletPool.Count == 0)
        {
            Debug.Log("创建新对象");
            CreateNewBullet();
        }

        GameObject bulletToGet = bulletPool.Dequeue();
        bulletToGet.SetActive(true);
        bulletToGet.transform.parent = null;

        return bulletToGet;
    }
    /// <summary>
    /// 回收子弹进对象池
    /// </summary>
    /// <param name="bullet"></param>
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
        bullet.transform.parent = transform;
    }
    /// <summary>
    /// 创建对象池
    /// </summary>
    private void CreateInitialPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewBullet();
        }
    }
    /// <summary>
    /// 在对象池中创建新的子弹对象
    /// </summary>
    private void CreateNewBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab, transform);
        newBullet.SetActive(false);
        bulletPool.Enqueue(newBullet);
    }
}
