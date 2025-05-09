using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：敌人盾牌类
//****************************************
public class Enemy_Shield : MonoBehaviour, IDamagable
{
    private Enemy_Melee enemy;

    [SerializeField]
    private int durability;//耐久度

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy_Melee>();

        durability = enemy.shieldDurability;
    }
    /// <summary>
    /// 减少耐久度
    /// </summary>
    public void ReduceDurability()
    {
        durability--;
        if (durability <= 0)
        {
            enemy.anim.SetFloat("ChaseIndex", 0);//恢复原来动画
            gameObject.SetActive(false);//隐藏盾牌
        }
    }

    public void TakeDamage()
    {
        ReduceDurability();
    }
}
