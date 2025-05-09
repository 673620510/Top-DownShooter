using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//****************************************
//创建人：逸龙
//功能说明：玩家准心类
//****************************************
public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [Header("Aim Viusal - Laser 激光瞄准视觉效果")]
    [SerializeField]
    private LineRenderer aimLaser;

    [Header("Aim control 瞄准控制器")]
    [SerializeField]
    private Transform aim;

    [SerializeField]
    private bool isAimingPrecisly;
    [SerializeField]
    private bool isLockingToTarget;

    [Header("Camera control 相机控制器")]
    [SerializeField]
    private Transform cameraTarget;
    [Range(.5f,1f)]
    [SerializeField]
    private float minCameraDistance;
    [Range(1f,3f)]
    [SerializeField]
    private float maxCameraDistance;
    [Range(3f,5f)]
    [SerializeField]
    private float cameraSensetivity;

    [Space]

    [SerializeField]
    private LayerMask aimLayerMask;

    private Vector2 mouseInput;
    private RaycastHit lastKnownMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }
    private void Update()
    {
        if (player.health.isDead) return;

        if (Input.GetKeyDown(KeyCode.P)) isAimingPrecisly = !isAimingPrecisly;
        if (Input.GetKeyDown(KeyCode.L)) isLockingToTarget = !isLockingToTarget;

        UpdateAimVisuals();
        UpdateAimPosition();
        UpdateCameraPosition();
    }
    /// <summary>
    /// 更新准心射线
    /// </summary>
    private void UpdateAimVisuals()
    {
        aimLaser.enabled = player.weapon.WeaponReady();

        if (!aimLaser.enabled) return;

        WeaponModel weaponModel = player.weaponVisuals.CurrentWeaponModel();

        weaponModel.transform.LookAt(aim);
        weaponModel.gunPoint.LookAt(aim);

        Transform gunPoint = player.weapon.GunPoint();
        Vector3 laserDirection = player.weapon.BulletDirection();

        float laserTipLenght = .5f;
        float gunDistance = player.weapon.CurrentWeapon().gunDistance;

        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;
            laserTipLenght = 0;
        }

        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLenght);
    }
    /// <summary>
    /// 更新准心坐标
    /// </summary>
    private void UpdateAimPosition()
    {
        Transform target = Target();
        if (target != null && isLockingToTarget)
        {
            if (target.GetComponent<Renderer>() != null)
            {
                aim.position = target.GetComponent<Renderer>().bounds.center;
            }
            else
            {
                aim.position = target.position;
            }
            return;
        }
        aim.position = GetMouseHitInfo().point;
        if (!isAimingPrecisly)
        {
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
        }
    }

    /// <summary>
    /// 获取目标点
    /// </summary>
    /// <returns></returns>
    public Transform Target()
    {
        Transform target = null;
        if (GetMouseHitInfo().transform.GetComponent<Target>() != null)
        {
            target = GetMouseHitInfo().transform;
        }
        return target;
    }
    public Transform Aim() => aim;
    /// <summary>
    /// 是否精准射击
    /// </summary>
    /// <returns></returns>
    public bool CanAimPrecisly() => isAimingPrecisly;
    /// <summary>
    /// 获取一条从摄像机射向鼠标位置的射线信息
    /// </summary>
    /// <returns></returns>
    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);
        if (Physics.Raycast(ray, out var hitInfo,Mathf.Infinity,aimLayerMask))
        {
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }

        return lastKnownMouseHit;
    }
    #region Camera Region
    /// <summary>
    /// 更新相机坐标
    /// </summary>
    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesieredCameraPosition(), cameraSensetivity * Time.deltaTime);
    }
    /// <summary>
    /// 获取受限的相机坐标
    /// </summary>
    /// <returns></returns>
    private Vector3 DesieredCameraPosition()
    {
        //对向下移动时的相机距离进行限制
        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesierdPosition = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesierdPosition, minCameraDistance, actualMaxCameraDistance);

        desiredCameraPosition = transform.position + aimDirection * clampedDistance;
        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;
    }
    #endregion
    /// <summary>
    /// 注册输入事件
    /// </summary>
    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Aim.performed += context => mouseInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => mouseInput = Vector2.zero;
    }
}
