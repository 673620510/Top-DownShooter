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
    private int poolSize = 10;//对象池容量

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();//对象池队列字典

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    /// <summary>
    /// 获取对象池中的对象
    /// </summary>
    /// <param name="poolName">对象池名称</param>
    /// <returns></returns>
    public GameObject GetObject(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab)) InitializeNewPool(prefab);

        if (poolDictionary[prefab].Count == 0) CreateNewObject(prefab);

        GameObject objectToGet = poolDictionary[prefab].Dequeue();
        objectToGet.SetActive(true);
        objectToGet.transform.parent = null;

        return objectToGet;
    }
    public void ReturnObject(GameObject objectToReturn, float delay = 0) => StartCoroutine(DelayReturn(delay, objectToReturn));
    /// <summary>
    /// 延迟回收
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator DelayReturn(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool(objectToReturn);
    }
    /// <summary>
    /// 回收对象进对象池
    /// </summary>
    /// <param name="objectToReturn"></param>
    private void ReturnToPool(GameObject objectToReturn)
    {
        GameObject originalPrefab = objectToReturn.GetComponent<PoolObject>().originalPerfab;

        objectToReturn.SetActive(false);
        objectToReturn.transform.parent = transform;

        poolDictionary[originalPrefab].Enqueue(objectToReturn);
    }
    /// <summary>
    /// 初始化新对象池
    /// </summary>
    private void InitializeNewPool(GameObject prefab)
    {
        poolDictionary[prefab] = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject(prefab);
        }
    }
    /// <summary>
    /// 在对象池中创建新的对象
    /// </summary>
    private void CreateNewObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, transform);
        newObject.AddComponent<PoolObject>().originalPerfab = prefab;
        newObject.SetActive(false);

        poolDictionary[prefab].Enqueue(newObject);
    }
}
