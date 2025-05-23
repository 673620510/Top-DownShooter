using System.Collections.Generic;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：关卡组件类
//****************************************
public class LevelPart : MonoBehaviour
{
    [Header("Intersection check")]
    [SerializeField]
    private LayerMask intersectionLayer;
    [SerializeField]
    private Collider[] intersectionColliders;
    [SerializeField]
    private Transform intersectionCheckParent;
    /// <summary>
    /// 是否检测到交集
    /// </summary>
    /// <returns></returns>
    public bool IntersectionDetected()
    {
        Physics.SyncTransforms();

        foreach (Collider collider in intersectionColliders)
        {
            Collider[] hitColliders = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity, intersectionLayer);
            
            foreach (Collider hit in hitColliders)
            {
                IntersectionCheck intersectionCheck = hit.GetComponentInParent<IntersectionCheck>();

                if (intersectionCheck != null && intersectionCheckParent != intersectionCheck.transform) return true;
            }
        }

        return false;
    }
    /// <summary>
    /// 对齐目标点
    /// </summary>
    /// <param name="targetSnapPoint"></param>
    public void SnapAndAlignPartTo(SnapPoint targetSnapPoint)
    {
        SnapPoint entrancePoint = GetEntrancePoint();

        AlignTo(entrancePoint, targetSnapPoint);
        SnapTo(entrancePoint, targetSnapPoint);
    }
    /// <summary>
    /// 旋转对齐目标点
    /// </summary>
    /// <param name="ownSnapPoint"></param>
    /// <param name="targetSnapPoint"></param>
    private void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        var rotationOffsetr = ownSnapPoint.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
        transform.rotation = targetSnapPoint.transform.rotation;
        transform.Rotate(0, 180, 0);
        transform.Rotate(0, -rotationOffsetr, 0);
    }
    /// <summary>
    /// 移动对齐目标点
    /// </summary>
    /// <param name="ownSnapPoint"></param>
    /// <param name="targetSnapPoint"></param>
    private void SnapTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        var offset = transform.position - ownSnapPoint.transform.position;
        var newPosition = targetSnapPoint.transform.position + offset;
        transform.position = newPosition;
    }
    /// <summary>
    /// 获取入口点
    /// </summary>
    /// <returns></returns>
    public SnapPoint GetEntrancePoint() => GetSnapPointOfType(SnapPointType.Enter);
    /// <summary>
    /// 获取出口点
    /// </summary>
    /// <returns></returns>
    public SnapPoint GetExitPoint() => GetSnapPointOfType(SnapPointType.Exit);
    /// <summary>
    /// 获取指定类型的连接点
    /// </summary>
    /// <param name="pointType"></param>
    /// <returns></returns>
    private SnapPoint GetSnapPointOfType(SnapPointType pointType)
    {
        SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
        List<SnapPoint> filteredSnapPoints = new List<SnapPoint>();

        foreach (SnapPoint snapPoint in snapPoints)
        {
            if (snapPoint.pointType == pointType)
            {
                filteredSnapPoints.Add(snapPoint);
            }
        }
        if (filteredSnapPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, filteredSnapPoints.Count);
            return filteredSnapPoints[randomIndex];
        }
        return null;
    }
}
