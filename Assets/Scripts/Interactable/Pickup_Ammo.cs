using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 弹药箱类型
/// </summary>
public enum AmmoBoxType
{
    smallBox,//小弹药箱
    bigBox//大弹药箱
}

[Serializable]
public struct AmmoData//弹药数据
{
    public WeaponType weaponType;//武器类型
    [Range(10, 100)]
    public int minAmount;//最小弹药数量
    [Range(10, 100)]
    public int maxAmount;//最大弹药数量
}

public class Pickup_Ammo : Interactable
{
    [SerializeField]
    private AmmoBoxType boxType;//弹药箱类型


    [SerializeField]
    private List<AmmoData> smallBoxAmmo;//小弹药箱
    [SerializeField]
    private List<AmmoData> bigBoxAmmo;//大弹药箱
    [SerializeField]
    private GameObject[] boxModel;//弹药箱模型

    private void Start()
    {
        SetupBoxModel();
    }
    public override void Interaction()
    {
        base.Interaction();

        List<AmmoData> currentAmmoList = smallBoxAmmo;
        if (boxType == AmmoBoxType.bigBox) currentAmmoList = bigBoxAmmo;

        foreach (AmmoData ammo in currentAmmoList)
        {
            Weapon weapon = weaponController.WeaponInSlots(ammo.weaponType);

            AddBulletsToWeapon(weapon, GetBulltetAmount(ammo));
        }

        ObjectPool.instance.ReturnObject(gameObject);
    }
    /// <summary>
    /// 获取弹药数量
    /// </summary>
    /// <param name="ammoData"></param>
    /// <returns></returns>
    private int GetBulltetAmount(AmmoData ammoData)
    {
        float min = Mathf.Min(ammoData.minAmount, ammoData.maxAmount);
        float max = Mathf.Max(ammoData.minAmount, ammoData.maxAmount);

        int randomAmmoAmount = UnityEngine.Random.Range(ammoData.minAmount, ammoData.maxAmount);

        return Mathf.RoundToInt(randomAmmoAmount);
    }
    /// <summary>
    /// 设置弹药箱模型
    /// </summary>
    private void SetupBoxModel()
    {
        for (int i = 0; i < boxModel.Length; i++)
        {
            boxModel[i].SetActive(false);

            if (i == (int)boxType)
            {
                boxModel[i].SetActive(true);
                UpdateMeshAndMaterial(boxModel[i].GetComponent<MeshRenderer>());
            }
        }
    }
    /// <summary>
    /// 往武器添加子弹
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="amount"></param>
    private void AddBulletsToWeapon(Weapon weapon, int amount)
    {
        if (weapon == null) return;

        weapon.totalReserveAmmo += amount;
    }
}
