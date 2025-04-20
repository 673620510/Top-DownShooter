using System;
using System.Collections.Generic;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：掩体类
//****************************************
public class Cover : MonoBehaviour
{
    private BoxCollider coverCollider;

    private Transform playerTransform;

    [Header("Cover point 掩体点位")]
    [SerializeField]
    private GameObject coverPointPrefab;
    [SerializeField]
    private List<CoverPoint> coverPoints = new List<CoverPoint>();
    [SerializeField]
    private float xOffset = 1;
    [SerializeField]
    private float yOffset = .2f;
    [SerializeField]
    private float zOffset = 1;
    [SerializeField]
    private float offset = 0.2f;

    private void Awake()
    {
        coverCollider = GetComponent<BoxCollider>();
        if (coverCollider == null)
        {
            Debug.LogError("Cover collider is missing!");
        }
    }
    void Start()
    {
        GenerateCoverPoints();

        playerTransform = FindFirstObjectByType<Player>().transform;
    }
    /// <summary>
    /// 生成掩体点
    /// </summary>
    private void GenerateCoverPoints()
    {
        Vector3[] localCoverPoints =
        {
            coverCollider.center + new Vector3(0, 0, coverCollider.size.z / 2 + offset),
            coverCollider.center + new Vector3(0, 0, -coverCollider.size.z / 2 - offset),
            coverCollider.center + new Vector3(coverCollider.size.x / 2 + offset, 0, 0),
            coverCollider.center + new Vector3(-coverCollider.size.x / 2 - offset, 0, 0),
            //new Vector3(0, yOffset, zOffset),
            //new Vector3(0, yOffset, -zOffset),
            //new Vector3(xOffset, yOffset, 0),
            //new Vector3(-xOffset, yOffset, 0),
        };

        foreach (Vector3 localPoint in localCoverPoints)
        {
            Vector3 worldPoint = transform.TransformPoint(localPoint);
            CoverPoint coverPoint = Instantiate(coverPointPrefab, worldPoint, Quaternion.identity, transform).GetComponent<CoverPoint>();
            coverPoints.Add(coverPoint);
        }
    }
    /// <summary>
    /// 获取有效的掩体点
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    public List<CoverPoint> GetValidCoverPoints(Transform enemy)
    {
        List<CoverPoint> validCoverPoints = new List<CoverPoint>();

        foreach (CoverPoint coverPoint in coverPoints)
        {
            if (IsValidCoverPoint(coverPoint, enemy))
            {
                validCoverPoints.Add(coverPoint);
            }
        }
        return validCoverPoints;
    }
    /// <summary>
    /// 判断掩体点是否有效
    /// </summary>
    /// <param name="coverPoint"></param>
    /// <param name="enemy"></param>
    /// <returns></returns>
    private bool IsValidCoverPoint(CoverPoint coverPoint, Transform enemy)
    {
        if (coverPoint.occupied) return false;

        if (!IsFutherestFromPlayer(coverPoint)) return false;

        if (IsCoverCloseToPlayer(coverPoint)) return false;

        if (IsCoverBehindPlayer(coverPoint, enemy)) return false;

        if (IsCoverCloseToLastCover(coverPoint, enemy)) return false;

        return true;
    }
    /// <summary>
    /// 判断掩体点是否离玩家最远
    /// </summary>
    /// <param name="coverPoint"></param>
    /// <returns></returns>
    private bool IsFutherestFromPlayer(CoverPoint coverPoint)
    {
        CoverPoint futherestPoint = null;
        float futherestDistance = 0f;
        foreach (CoverPoint point in coverPoints)
        {
            float distance = Vector3.Distance(point.transform.position, playerTransform.position);
            if (distance > futherestDistance)
            {
                futherestDistance = distance;
                futherestPoint = point;
            }
        }
        return futherestPoint == coverPoint;
    }
    /// <summary>
    /// 判断掩体点是否在玩家后面
    /// </summary>
    /// <param name="coverPoint"></param>
    /// <param name="enemy"></param>
    /// <returns></returns>
    private bool IsCoverBehindPlayer(CoverPoint coverPoint, Transform enemy)
    {
        float distanceToPlayer = Vector3.Distance(coverPoint.transform.position, playerTransform.position);
        float distanceToEnemy = Vector3.Distance(coverPoint.transform.position, enemy.position);

        return distanceToPlayer < distanceToEnemy;
    }
    /// <summary>
    /// 判断掩体点是否离玩家太近
    /// </summary>
    /// <param name="coverPoint"></param>
    /// <returns></returns>
    private bool IsCoverCloseToPlayer(CoverPoint coverPoint)
    {
        float distanceToPlayer = Vector3.Distance(coverPoint.transform.position, playerTransform.position);
        return distanceToPlayer < 2f;
    }
    /// <summary>
    /// 判断掩体点是否离上一个掩体太近
    /// </summary>
    /// <param name="coverPoint"></param>
    /// <param name="enemy"></param>
    /// <returns></returns>
    private bool IsCoverCloseToLastCover(CoverPoint coverPoint,Transform enemy)
    {
        CoverPoint lastCover = enemy.GetComponent<Enemy_Range>().currentCover;

        return lastCover != null && Vector3.Distance(coverPoint.transform.position, lastCover.transform.position) < 3;
    }
}
