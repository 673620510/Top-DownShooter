using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：敌人武器模型类
//****************************************
public class Enemy_WeaponModel : MonoBehaviour
{
    public Enemy_MeleeWeaponType weaponType;//武器类型
    public AnimatorOverrideController overrideController;//动画覆盖控制器
    public Enemy_MeleeWeaponData weaponData;//武器数据

    [SerializeField]
    private GameObject[] trailEffects;//拖尾特效

    [Header("Damage atributes")]
    public Transform[] damagePoints;
    public float attackRadius;

    [ContextMenu("Assign damage point transforms")]
    private void GetDamagePoints()
    {
        damagePoints = new Transform[trailEffects.Length];
        for (int i = 0; i < trailEffects.Length; i++)
        {
            damagePoints[i] = trailEffects[i].transform;
        }
    }

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
    private void OnDrawGizmos()
    {
        if (damagePoints.Length > 0)
        {
            foreach (Transform point in damagePoints)
            {
                Gizmos.DrawWireSphere(point.position, attackRadius);
            }
        }
    }
}
