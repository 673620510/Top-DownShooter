using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：远程敌人武器模型类
//****************************************
/// <summary>
/// 远程敌人武器握持类型
/// </summary>
public enum Enemy_RangeWeaponHoldType
{
    Common,//普通握持
    LowHold,//低位握持
    HighHold//高位握持
}
public class Enemy_RangeWeaponModel : MonoBehaviour
{
    public Transform gunPoint;//枪口

    [Space]
    public Enemy_RangeWeaponType weaponType;//武器类型
    public Enemy_RangeWeaponHoldType weaponHoldType;//武器握把高度

    public Transform leftHandTarget;//左手IK目标
    public Transform leftElbowTarget;//左手肘部IK目标

}
