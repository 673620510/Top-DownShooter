using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：掩体点类
//****************************************
public class CoverPoint : MonoBehaviour
{
    public bool occupied;//是否被占用
    /// <summary>
    /// 设置占用状态
    /// </summary>
    /// <param name="occupied"></param>
    public void SetOccipied(bool occupied) => this.occupied = occupied;
}
