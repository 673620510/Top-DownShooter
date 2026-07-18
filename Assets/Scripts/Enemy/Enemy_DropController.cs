using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：敌人掉落控制器
//****************************************
public class Enemy_DropController : MonoBehaviour
{
    [SerializeField]
    private GameObject missionObjectKey;//钥匙对象

    public void Givekey(GameObject newKey) => missionObjectKey = newKey;
    /// <summary>
    /// 掉落物
    /// </summary>
    public void DropItems()
    {
        if (missionObjectKey != null) CreateItem(missionObjectKey);
        Debug.Log("dropped some items 掉落一些物品");
    }
    /// <summary>
    /// 创建物品
    /// </summary>
    /// <param name="go"></param>
    private void CreateItem(GameObject go)
    {
        GameObject newItem = Instantiate(go, transform.position + Vector3.up, Quaternion.identity);
    }
}
