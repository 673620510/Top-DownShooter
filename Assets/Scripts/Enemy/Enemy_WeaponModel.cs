using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：敌人武器模型类
//****************************************
public class Enemy_WeaponModel : MonoBehaviour
{
    public Enemy_MeleeWeaponType weaponType;//武器类型
    public AnimatorOverrideController overrideController;
    public Enemy_MeleeWeaponData weaponData;//武器数据

    [SerializeField]
    private GameObject[] trailEffects;//拖尾特效


    /// <summary>
    /// 启用武器拖尾特效
    /// </summary>
    /// <param name="enable"></param>
    public void EnableTrailEffect(bool enable)
    {
        foreach (var effect in trailEffects)
        {
            effect.SetActive(enable);
        }
    }
}
