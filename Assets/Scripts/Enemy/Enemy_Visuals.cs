using System.Collections.Generic;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：敌人视觉效果类
//****************************************
public enum Enemy_MeleeWeaponType
{
    OneHand,//单手武器
    Throw//投掷武器
}
public class Enemy_Visuals : MonoBehaviour
{
    [Header("Weapon model 武器模型")]
    [SerializeField]
    private Enemy_WeaponModel[] weaponModels;//武器模型
    [SerializeField]
    private Enemy_MeleeWeaponType weaponType;//武器类型
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

    private void Awake()
    {
        weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);

        CollectCorruptionCrystals();
    }


    private void Start()
    {
        //InvokeRepeating(nameof(SetupLook), 0, 1.5f);//每秒调用一次        
    }
    public void SetupWeaponType(Enemy_MeleeWeaponType type) => weaponType = type;
    /// <summary>
    /// 设置外观
    /// </summary>
    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
        SetupRandomCorrution();
    }
    /// <summary>
    /// 设置随机腐化水晶
    /// </summary>
    private void SetupRandomCorrution()
    {
        List<int> avalibleIndexs = new List<int>();

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
        foreach (var weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);
        }

        List<Enemy_WeaponModel> filteredWeaponModels = new List<Enemy_WeaponModel>();

        foreach (var weaponmodel in weaponModels)
        {
            if (weaponmodel.weaponType == weaponType)
            {
                filteredWeaponModels.Add(weaponmodel);
            }
        }

        int randomIndex = Random.Range(0, filteredWeaponModels.Count);

        currentWeaponModel = filteredWeaponModels[randomIndex].gameObject;
        currentWeaponModel.SetActive(true);
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
    /// 获取角色身上所有腐化水晶
    /// </summary>
    private void CollectCorruptionCrystals()
    {
        Enemy_CorruptionCrystal[] crystalComponents = GetComponentsInChildren<Enemy_CorruptionCrystal>(true);
        corruptionCrystals = new GameObject[crystalComponents.Length];

        for (int i = 0; i < crystalComponents.Length; i++)
        {
            corruptionCrystals[i] = crystalComponents[i].gameObject;
        }
    }
}
