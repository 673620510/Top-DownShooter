using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：连接点类
//****************************************
public enum SnapPointType
{
    Enter,
    Exit
}
public class SnapPoint : MonoBehaviour
{
    public SnapPointType pointType;//点类型
    private void OnValidate()
    {
        gameObject.name = "SnapPoint - " + pointType.ToString();
    }
}
