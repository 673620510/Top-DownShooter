using Cinemachine;
using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer transposer;//取景置换器（跟随目标移动，并在屏幕空间保持相机和跟随目标的相对位置）

    private float targetCameraDistance;//目标相机距离
    [SerializeField]
    private float distanceChangeRate;//距离变化率

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("You had more than one Camera");
            Destroy(gameObject);
        }

        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    private void Update()
    {
        float currentDistance = transposer.m_CameraDistance;

        if (Mathf.Abs(targetCameraDistance) - currentDistance <= 0.01f) return;

        transposer.m_CameraDistance = Mathf.Lerp(currentDistance, targetCameraDistance, distanceChangeRate * Time.deltaTime);
    }
    /// <summary>
    /// 改变相机距离
    /// </summary>
    /// <param name="distance">距离</param>
    public void ChangeCamerDistance(float distance) => targetCameraDistance = distance;
}
