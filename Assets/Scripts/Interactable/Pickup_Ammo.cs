using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ҩ������
/// </summary>
public enum AmmoBoxType
{
    smallBox,//С��ҩ��
    bigBox//��ҩ��
}

[Serializable]
public struct AmmoData//��ҩ����
{
    public WeaponType weaponType;//��������
    [Range(10, 100)]
    public int minAmount;//��С��ҩ����
    [Range(10, 100)]
    public int maxAmount;//���ҩ����
}

public class Pickup_Ammo : Interactable
{
    [SerializeField]
    private AmmoBoxType boxType;//��ҩ������


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

            AddBulletsToWeapon(weapon, GetBulltetAmount(ammo));
        }

        ObjectPool.instance.ReturnObject(gameObject);
    }
    /// <summary>
    /// ��ȡ��ҩ����
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
    /// �����������ӵ�
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="amount"></param>
    private void AddBulletsToWeapon(Weapon weapon, int amount)
    {
        if (weapon == null) return;

        weapon.totalReserveAmmo += amount;
    }
}
