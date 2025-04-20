using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

//****************************************
//创建人：逸龙
//功能说明：敌人视觉效果类
//****************************************
public enum Enemy_MeleeWeaponType
{
    OneHand,//单手武器
    Throw,//投掷武器
    Unarmed//无武器
}
public enum Enemy_RangeWeaponType
{
    Pistol,//手枪
    Revolver,//左轮
    Shotgun,//霰弹枪
    AutoRifle,//自动步枪
    Rifle//步枪
}
public class Enemy_Visuals : MonoBehaviour
{
    public GameObject currentWeaponModel { get; private set; }//当前武器模型

    [Header("Corruption visuals 腐化视觉效果")]
    [SerializeField]
    private GameObject[] corruptionCrystals;//腐化水晶
    [SerializeField]
    private int corruptionAmount;//腐化数量

    [Header("Color 颜色")]
    [SerializeField]
    private Texture[] colorTextures;//颜色贴图
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;//渲染器

    [Header("Rig references 绑定参考")]
    [SerializeField]
    private Transform leftHandIK;//左手IK
    [SerializeField]
    private Transform leftElbowIK;//左手肘部IK
    [SerializeField]
    private TwoBoneIKConstraint leftHandIKConstraint;//左手IK约束
    [SerializeField]
    private MultiAimConstraint weaponAim;//武器瞄准约束

    private void Start()
    {
        //InvokeRepeating(nameof(SetupLook), 0, 1.5f);//每秒调用一次        
    }
    /// <summary>
    /// 启用武器拖尾
    /// </summary>
    /// <param name="enable"></param>
    public void EnableWeaponTrail(bool enable)
    {
        currentWeaponModel.GetComponent<Enemy_WeaponModel>()?.EnableTrailEffect(enable);
    }
    /// <summary>
    /// 设置外观
    /// </summary>
    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
        SetupRandomCorrution();
        EnableWeaponTrail(false);
    }
    /// <summary>
    /// 设置随机腐化水晶
    /// </summary>
    private void SetupRandomCorrution()
    {
        List<int> avalibleIndexs = new List<int>();
        corruptionCrystals = CollectCorruptionCrystals();

        for (int i = 0; i < corruptionCrystals.Length; i++)
        {
            avalibleIndexs.Add(i);
            corruptionCrystals[i].SetActive(false);
        }

        for (int i = 0; i < corruptionAmount; i++)
        {
            if (avalibleIndexs.Count == 0) break;

            int randomIndex = Random.Range(0, avalibleIndexs.Count);

            int objectIndex = avalibleIndexs[randomIndex];

            corruptionCrystals[objectIndex].SetActive(true);
            avalibleIndexs.RemoveAt(randomIndex);
        }
    }
    /// <summary>
    /// 设置随机武器
    /// </summary>
    private void SetupRandomWeapon()
    {
        bool thisEnemyIsMelee = GetComponent<Enemy_Melee>() != null;
        bool thisEnemyIsRange = GetComponent<Enemy_Range>() != null;

        if (thisEnemyIsMelee) currentWeaponModel = FindMeleeWeaponModel();

        if (thisEnemyIsRange) currentWeaponModel = FindRangeWeaponModel();

        if (currentWeaponModel == null) return;

        currentWeaponModel.SetActive(true);

        OverrideAnimatorControllerIfCan();
    }

    /// <summary>
    /// 设置随机颜色
    /// </summary>
    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTextures.Length);

        Material newMat = new Material(skinnedMeshRenderer.material);

        newMat.mainTexture = colorTextures[randomIndex];

        skinnedMeshRenderer.material = newMat;
    }
    /// <summary>
    /// 查找远程武器模型
    /// </summary>
    /// <returns></returns>
    private GameObject FindRangeWeaponModel()
    {
        Enemy_RangeWeaponModel[] weaponModels = GetComponentsInChildren<Enemy_RangeWeaponModel>(true);
        Enemy_RangeWeaponType weaponType = GetComponent<Enemy_Range>().weaponType;

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
            {
                SwitchAnimationLayer((int)(weaponModel.weaponHoldType));
                SetupLeftHandIK(weaponModel.leftHandTarget, weaponModel.leftElbowTarget);
                return weaponModel.gameObject;
            }
        }
        return null;
    }
    /// <summary>
    /// 查找近战武器模型
    /// </summary>
    /// <returns></returns>
    private GameObject FindMeleeWeaponModel()
    {
        Enemy_WeaponModel[] weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);
        Enemy_MeleeWeaponType weaponType = GetComponent<Enemy_Melee>().weaponType;
        List<Enemy_WeaponModel> filteredWeaponModels = new List<Enemy_WeaponModel>();

        foreach (var weaponmodel in weaponModels)
        {
            if (weaponmodel.weaponType == weaponType)
            {
                filteredWeaponModels.Add(weaponmodel);
            }
        }

        int randomIndex = Random.Range(0, filteredWeaponModels.Count);

        return currentWeaponModel = filteredWeaponModels[randomIndex].gameObject;
    }

    /// <summary>
    /// 获取角色身上所有腐化水晶
    /// </summary>
    private GameObject[] CollectCorruptionCrystals()
    {
        Enemy_CorruptionCrystal[] crystalComponents = GetComponentsInChildren<Enemy_CorruptionCrystal>(true);
        GameObject[] corruptionCrystals = new GameObject[crystalComponents.Length];

        for (int i = 0; i < crystalComponents.Length; i++)
        {
            corruptionCrystals[i] = crystalComponents[i].gameObject;
        }
        return corruptionCrystals;
    }
    /// <summary>
    /// 覆盖动画控制器
    /// </summary>
    private void OverrideAnimatorControllerIfCan()
    {
        AnimatorOverrideController overrideController = currentWeaponModel.GetComponent<Enemy_WeaponModel>()?.overrideController;

        if (overrideController != null)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController = overrideController;
        }
    }
    /// <summary>
    /// 切换动画层级
    /// </summary>
    /// <param name="layerIndex"></param>
    private void SwitchAnimationLayer(int layerIndex)
    {
        Animator anim = GetComponentInChildren<Animator>();
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(layerIndex, 1);
    }
    /// <summary>
    /// 启用或禁用IK
    /// </summary>
    /// <param name="enable"></param>
    public void EnableIK(bool enableLeftHand, bool enableAim)
    {
        //rig.weight = enable ? 1 : 0;
        leftHandIKConstraint.weight = enableLeftHand ? 1 : 0;
        weaponAim.weight = enableAim ? 1 : 0;
    }
    /// <summary>
    /// 设置左手IK
    /// </summary>
    /// <param name="leftHandTarget"></param>
    /// <param name="leftElbowTarget"></param>
    private void  SetupLeftHandIK(Transform leftHandTarget, Transform leftElbowTarget)
    {
        leftHandIK.localPosition = leftHandTarget.localPosition;
        leftHandIK.localRotation = leftHandTarget.localRotation;

        leftElbowIK.localPosition = leftElbowTarget.localPosition;
        leftElbowIK.localRotation = leftElbowTarget.localRotation;
    }
}
