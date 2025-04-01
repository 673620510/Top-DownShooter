using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 弹药箱类型
/// </summary>
public enum AmmoBoxType
{
    smallBox,
    bigBox
}

public class Pickup_Ammo : Interactable
{
    private PlayerWeaponController weaponController;

    [SerializeField]
    private AmmoBoxType boxType;//弹药箱类型
    [Serializable]
    public struct AmmoData//弹药数据
    {
        public WeaponType weaponType;//武器类型
        public int amount;//数量
    }

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

            AddBulletsToWeapon(weapon, ammo.amount);
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(weaponController == null) weaponController = other.GetComponent<PlayerWeaponController>();
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
