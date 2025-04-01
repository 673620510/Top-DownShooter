using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ҩ������
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
    private AmmoBoxType boxType;//��ҩ������
    [Serializable]
    public struct AmmoData//��ҩ����
    {
        public WeaponType weaponType;//��������
        public int amount;//����
    }

    [SerializeField]
    private List<AmmoData> smallBoxAmmo;//С��ҩ��
    [SerializeField]
    private List<AmmoData> bigBoxAmmo;//��ҩ��
    [SerializeField]
    private GameObject[] boxModel;//��ҩ��ģ��
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
    /// ���õ�ҩ��ģ��
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
    /// ����������ӵ�
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="amount"></param>
    private void AddBulletsToWeapon(Weapon weapon, int amount)
    {
        if (weapon == null) return;

        weapon.totalReserveAmmo += amount;
    }
}
