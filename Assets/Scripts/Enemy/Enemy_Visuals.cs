using System.Collections.Generic;
using UnityEngine;

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
    private GameObject FindRangeWeaponModel()
    {
        Enemy_RangeWeaponModel[] weaponModels = GetComponentsInChildren<Enemy_RangeWeaponModel>(true);
        Enemy_RangeWeaponType weaponType = GetComponent<Enemy_Range>().weaponType;

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
            {
                return weaponModel.gameObject;
            }
        }
        return null;
    }

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

    private void OverrideAnimatorControllerIfCan()
    {
        AnimatorOverrideController overrideController = currentWeaponModel.GetComponent<Enemy_WeaponModel>()?.overrideController;

        if (overrideController != null)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController = overrideController;
        }
    }
}
