using Cinemachine;
using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer transposer;//ȡ���û���������Ŀ���ƶ���������Ļ�ռ䱣������͸���Ŀ������λ�ã�

    private float targetCameraDistance;//Ŀ���������
    [SerializeField]
    private float distanceChangeRate;//����仯��

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
    /// �ı��������
    /// </summary>
    /// <param name="distance">����</param>
    public void ChangeCamerDistance(float distance) => targetCameraDistance = distance;
}
