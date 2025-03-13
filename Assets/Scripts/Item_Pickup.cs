using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//****************************************
//创建人：逸龙
//功能说明：用于标记可拾取的物品类
//****************************************
public class Item_Pickup : MonoBehaviour
{
    [SerializeField]
    private Weapon_Data weaponData;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponController>()?.PickupWeapon(weaponData);
    }
}
