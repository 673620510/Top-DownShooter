using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//****************************************
//创建人：逸龙
//功能说明：武器模型类
//****************************************
/// <summary>
/// 拾取位置枚举
/// </summary>
public enum EquipType { SideEquipAnimation, BackEquipAnimation };
/// <summary>
/// 握把高度枚举
/// </summary>
public enum HoldType { CommonHold = 1, LowHold, HighHold };
public class WeaponModel : MonoBehaviour
{
    public WeaponType weaponType;
    public EquipType equipAnimationType;
    public HoldType holdType;

    public Transform gunPoint;
    public Transform holdPoint;
}
