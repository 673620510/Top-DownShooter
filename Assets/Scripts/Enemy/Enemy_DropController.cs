using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Enemy_DropController : MonoBehaviour
{
    public void DropItems()
    {
        Debug.Log("dropped some items");
    }
    private void CreateItem(GameObject go)
    {
        GameObject newItem = Instantiate(go, transform.position + Vector3.up, Quaternion.identity);
    }
}
