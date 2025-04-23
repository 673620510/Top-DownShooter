using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：敌人动画事件类
//****************************************
public class Enemy_AnimationEvents : MonoBehaviour
{
    private Enemy enemy;
    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    /// <summary>
    /// 动画触发器
    /// </summary>
    public void AnimationTrigger() => enemy.AnimationTrigger();
    /// <summary>
    /// 开启手动移动
    /// </summary>
    public void StartManualMovement() => enemy.ActivateManualMovement(true);
    /// <summary>
    /// 停止手动移动
    /// </summary>
    public void StopManualMovement() => enemy.ActivateManualMovement(false);
    /// <summary>
    /// 开启手动旋转
    /// </summary>
    public void StartManualRotation() => enemy.ActivateManualRotation(true);
    /// <summary>
    /// 停止手动旋转
    /// </summary>
    public void StopManualRotation() => enemy.ActivateManualRotation(false);
    /// <summary>
    /// 特殊能力事件
    /// </summary>
    public void AbilityEvent() => enemy.AbilityTrigger();
    public void EnableIK() => enemy.visuals.EnableIK(true, true, 1f);
    public void EnableWeaponModel()
    {
        enemy.visuals.EnableWeaponModel(true);
        enemy.visuals.EnableSeconnderyWeaponModel(false);
    }
}
