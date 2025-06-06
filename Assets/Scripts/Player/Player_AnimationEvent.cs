using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//****************************************
//创建人：逸龙
//功能说明：玩家动画事件类
//****************************************
public class Player_AnimationEvent : MonoBehaviour
{
    private Player_WeaponVisuals visualController;
    private Player_WeaponController weaponController;
    private void Start()
    {
        visualController = GetComponentInParent<Player_WeaponVisuals>();
        weaponController = GetComponentInParent<Player_WeaponController>();
    }
    /// <summary>
    /// 装弹完毕
    /// </summary>
    public void ReloadIsOver()
    {
        visualController.MaximizeRigWeigtht();
        weaponController.CurrentWeapon().RefillBullets();
        weaponController.SetWeaponReady(true);
    }
    /// <summary>
    /// 骨骼回正
    /// </summary>
    public void ReturnRig()
    {
        visualController.MaximizeRigWeigtht();
        visualController.MaximizeLeftHandWeight();
    }
    /// <summary>
    /// 切换武器完毕
    /// </summary>
    public void WeaponEquipingIsOver()
    {
        weaponController.SetWeaponReady(true);
    }
    public void SwitchOnWeaponModel() => visualController.SwitchOnCurrentWeaponModel();
}
