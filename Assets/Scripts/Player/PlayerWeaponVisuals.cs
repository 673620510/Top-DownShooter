using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
//****************************************
//创建人：逸龙
//功能说明：玩家武器视图类
//****************************************
public class PlayerWeaponVisuals : MonoBehaviour
{
    private Player player;
    private Animator anim;

    [SerializeField]
    private WeaponModel[] weaponModels;//武器模型数组
    [SerializeField]
    private BackupWeaponModel[] backupWeaponModels;//备用武器模型数组

    [Header("Rig")]
    [SerializeField]
    private float rigWeightIncreaseRate;
    private bool shouldIncrease_RigWeight;
    private Rig rig;

    [Header("Left hand IK")]
    [SerializeField]
    private float leftHandIKWeightIncreaseRate;
    [SerializeField]
    private TwoBoneIKConstraint leftHandIK;
    [SerializeField]
    private Transform leftHandIK_Target;
    private bool shouldIncrease_LeftHandIKWieght;


    private void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);
    }
    private void Update()
    {
        UpdateRigWigth();
        UpdateLeftHandIKWeight();
    }
    /// <summary>
    /// 播放换弹动画
    /// </summary>
    public void PlayerReloadAnimation()
    {
        float reloadSpeed = player.weapon.CurrentWeapon().reloadSpeed;

        anim.SetFloat("ReloadSpeed", reloadSpeed);
        anim.SetTrigger("Reload");
        ReduceRigWeight();
    }
    /// <summary>
    /// 播放开火动画
    /// </summary>
    public void PlayFireAnimation() => anim.SetTrigger("Fire");
    /// <summary>
    /// 播放拾取武器动画
    /// </summary>
    public void PlayWeaponEquipAnimation()
    {
        EquipType equipType = CurrentWeaponModel().equipAnimationType;

        float equipmentSpeed = player.weapon.CurrentWeapon().equipmentSpeed;

        leftHandIK.weight = 0;
        ReduceRigWeight();
        anim.SetTrigger("EquipWeapon");
        anim.SetFloat("EquipType", (float)equipType);
        anim.SetFloat("EquipSpeed", equipmentSpeed);
    }
    /// <summary>
    /// 开始切换武器模型
    /// </summary>
    public void SwitchOnCurrentWeaponModel()
    {
        int animationIndex = (int)CurrentWeaponModel().holdType;

        SwitchOffWeaponModels();

        SwitchOffBackupWeaponModels();
        if (!player.weapon.HasOnlyOneWeapon()) SwitchOnBackupWeaponModel();

        SwitchAnimationLayer(animationIndex);
        CurrentWeaponModel().gameObject.SetActive(true);
        AttachLeftHand();
    }
    /// <summary>
    /// 隐藏所有枪械模型
    /// </summary>
    public void SwitchOffWeaponModels()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 开始切换备用武器模型
    /// </summary>
    public void SwitchOnBackupWeaponModel()
    {
        WeaponType weaponType = player.weapon.BackupWeapon().weaponType;

        foreach (BackupWeaponModel backupModel in backupWeaponModels)
        {
            if (backupModel.weaponType == weaponType) backupModel.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 隐藏所有备用武器模型
    /// </summary>
    private void SwitchOffBackupWeaponModels()
    {
        foreach (BackupWeaponModel backupModel in backupWeaponModels)
        {
            backupModel.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 切换动画层级
    /// </summary>
    /// <param name="layerIndex"></param>
    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(layerIndex, 1);
    }
    /// <summary>
    /// 获取当前武器的模型信息
    /// </summary>
    /// <returns></returns>
    public WeaponModel CurrentWeaponModel()
    {
        WeaponModel weaponModel = null;

        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;

        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i].weaponType == weaponType)
            {
                weaponModel = weaponModels[i];
            }
        }
        return weaponModel;
    }

    #region Animation Rigging Methods 动画骨骼权重设置
    /// <summary>
    /// 设置左手IK
    /// </summary>
    private void AttachLeftHand()
    {
        Transform targetTransfotm = CurrentWeaponModel().holdPoint;
        leftHandIK_Target.localPosition = targetTransfotm.localPosition;
        leftHandIK_Target.localRotation = targetTransfotm.localRotation;
    }
    /// <summary>
    /// 更新左手IK权重
    /// </summary>
    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncrease_LeftHandIKWieght)
        {
            leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;
            if (leftHandIK.weight >= 1)
            {
                shouldIncrease_LeftHandIKWieght = false;
            }
        }
    }
    /// <summary>
    /// 更新骨骼权重
    /// </summary>
    private void UpdateRigWigth()
    {
        if (shouldIncrease_RigWeight)
        {
            rig.weight += rigWeightIncreaseRate * Time.deltaTime;
            if (rig.weight >= 1)
            {
                shouldIncrease_RigWeight = false;
            }
        }
    }
    /// <summary>
    /// 减少骨骼权重
    /// </summary>
    private void ReduceRigWeight()
    {
        rig.weight = .15f;
    }
    /// <summary>
    /// 最大化骨骼权重
    /// </summary>
    public void MaximizeRigWeigtht() => shouldIncrease_RigWeight = true;
    /// <summary>
    /// 最大化左手IK权重
    /// </summary>
    public void MaximizeLeftHandWeight() => shouldIncrease_LeftHandIKWieght = true;
    #endregion
}